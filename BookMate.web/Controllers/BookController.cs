using BookMate.web.Core.Models;
using BookMate.web.Core.ViewModels.Book;
using BookMate.web.Data;
using BookMate.web.Extensions;
using BookMate.web.Interfaces;
using BookMate.web.Repositories;
using BookMate.web.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookMate.web.Controllers
{
    [Authorize(Roles =AppRoles.Archive)]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        
        public BookController(IUnitOfWork unitOfWork, IImageService imageService , IMapper mapper)
        {
            _unitOfWork = unitOfWork ;
            _imageService = imageService ;
            _mapper = mapper;
           
        }

        public async Task<IActionResult> Index(string SearchTerm , int PageNumber = 1 , int  PageSize = 5)
        {
            BookViewModel model = new BookViewModel()
            {
                Books = await _unitOfWork.Books.GetBooksAsync(SearchTerm , PageNumber, PageSize),      
            };
            return View(model);
        }

        public  IActionResult Details(int id , int page)
        {
            var book =      _unitOfWork.Books.GetBookDetails(id);
            book.page = page;
         
           return View(book);
        }

        public IActionResult Create(int page)
        {
            BookFormViewModel model = _unitOfWork.Books.GetModel();
            model.page = page;
            return View(model);
        }

        public async Task<IActionResult> SaveCreate(BookFormViewModel model)
        {
            if (model.AuthorId == 0)
            {
                ModelState.AddModelError("AuthorId", "Select Author");
            }
            if (ModelState.IsValid) 
            {
                var book = _mapper.Map<Book>(model);
                if (model.Image != null)
                {
                    var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                    var imagePath = "/images/book";

                    var (isUploaded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, imagePath, hasThumbnail: false);

                    if (!isUploaded)
                    {
                        ModelState.AddModelError("Image", errorMessage!);
                        return View("Create", _unitOfWork.Books.GetModel(model));
                    }
                    book.ImageUrl = imageName;
                }
                foreach (var category in model.SelectedCategories) 
                {
                    var bookCategory = new BookCategory
                    {
                        CategoryId = category
                    };
                    book.Categories.Add(bookCategory);
                }
                book.CreatedById = User.GetUserId();
               await _unitOfWork.Books.AddAsync(book);
               await  _unitOfWork.CompleteAsync();
                model.page = model.page == 0 ? 1 : model.page;
                return RedirectToAction("Index" , new { PageNumber = model.page});
            }
            model = _unitOfWork.Books.GetModel(model);
            return View("Create",model);
        }

   
        public IActionResult Edit(int id , int page) 
        {
            Book? book =  _unitOfWork.Books.GetBook(id).SingleOrDefault();
            BookFormViewModel model = _mapper.Map<BookFormViewModel>(book);
            model = _unitOfWork.Books.GetModel(model);
            model.page = page;
            return View(model);
        }

        public async Task< IActionResult> SaveEdit(BookFormViewModel model)
        {
            if(model.AuthorId == 0)
            {
                ModelState.AddModelError("AuthorId", "Select Author");
            }

            if (ModelState.IsValid)
            {
                var book =  _unitOfWork.Books.GetBook(model.Id).SingleOrDefault();
                //if user send a photo 
                if (model.Image != null)
                {
                    // if the user have a photo
                    if (!string.IsNullOrEmpty(book.ImageUrl))
                    {
                      _imageService.Delete($"/images/book/{book.ImageUrl}");
                    }

                    // the same steps of create 
                    var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                    var imagePath = "/images/book";

                    var (isUploaded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, imagePath, hasThumbnail: false);

                    if (!isUploaded)
                    {
                        ModelState.AddModelError("Image", errorMessage!);
                        return View("Edit", _unitOfWork.Books.GetModel(model));
                    }
                    book.ImageUrl = imageName;
                }
                else
                {
                    if ( model.IsDeletedImg is true && !string.IsNullOrEmpty(book.ImageUrl))
                    {
                        _imageService.Delete($"/images/book/{book.ImageUrl}");
                        book.ImageUrl = null;
                    }
                   
                }

                _mapper.Map(model, book);

                book.Categories.Clear();
                foreach (var category in model.SelectedCategories)
                {
                    book.Categories.Add(new BookCategory { CategoryId = category });
                }
                book.LastUpdatedOn = DateTime.UtcNow;
                book.LastUpdatedById = User.GetUserId();
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index", new { pageNumber = model.page });
            }
            model = _unitOfWork.Books.GetModel(model);
            return View("Edit", model);
        }

        public async Task< IActionResult> Delete(int id, int page) 
        {
          var book = await _unitOfWork.Books.GetByIdAsync(id);
            book.IsDeleted = !book.IsDeleted;
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index", new { pageNumber = page });

        }
        public async Task< IActionResult> AddCopy(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            book.Copy++;
            await _unitOfWork.CompleteAsync();
            return Json(book.Copy);
        }

    }
}
