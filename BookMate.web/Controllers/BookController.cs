using BookMate.web.Core.Models;
using BookMate.web.Core.ViewModels.Book;
using BookMate.web.Data;
using BookMate.web.Interfaces;
using BookMate.web.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookMate.web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepo _bookrepo;
        private readonly IMapper _mapper;
        private readonly ImageOperation _imageoperation;
        private  List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private  int _maxAllowedSize = 2097152;

        public BookController(IBookRepo bookrepo,IMapper mapper, ImageOperation imageoperation)
        {
            _bookrepo = bookrepo;
            _mapper = mapper;
            _imageoperation = imageoperation;
        }

        public async Task<IActionResult> Index(string SearchTerm , int PageNumber = 1 , int  PageSize = 5)
        {
            BookViewModel model = new BookViewModel()
            {
                Books = await _bookrepo.GetBooksAsync(SearchTerm , PageNumber, PageSize),      
            };
            return View(model);
        }

        public IActionResult Create(int page)
        {
            BookFormViewModel model = _bookrepo.GetModel();
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
                    var extension = Path.GetExtension(model.Image.FileName);
                    if (!_allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(nameof(model.Image), "Not allowed extension");
                    }

                    if (model.Image.Length > _maxAllowedSize)
                    {
                        ModelState.AddModelError(nameof(model.Image), "Max Size");
                    }
                    var imagename =  _imageoperation.Save(model.Image, "book");
                    book.ImageUrl = imagename;
                }
                foreach (var category in model.SelectedCategories) 
                {
                    var bookCategory = new BookCategory
                    {
                        CategoryId = category
                    };
                    book.Categories.Add(bookCategory);
                }
               await _bookrepo.AddAsync(book);
               await  _bookrepo.SaveAsync();
                model.page = model.page == 0 ? 1 : model.page;
                return RedirectToAction("Index" , new { PageNumber = model.page});
            }
            model = _bookrepo.GetModel(model);
            return View("Create",model);
        }

   
        public IActionResult Edit(int id , int page) 
        {
            Book? book =  _bookrepo.GetBook(id).SingleOrDefault();
            BookFormViewModel model = _mapper.Map<BookFormViewModel>(book);
            model = _bookrepo.GetModel(model);
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
                var book =  _bookrepo.GetBook(model.Id).SingleOrDefault();
                //if user send a photo 
                if (model.Image != null)
                
                {
                    // if the user have a photo
                    if (!string.IsNullOrEmpty(book.ImageUrl))
                    {
                      _imageoperation.RemoveImage(book.ImageUrl , "book");
                    }

                    // the same steps of create 
                    var extension = Path.GetExtension(model.Image.FileName);

                    if (!_allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(nameof(model.Image), "Not allowed extension");
                    }

                    if (model.Image.Length > _maxAllowedSize)
                    {
                        ModelState.AddModelError(nameof(model.Image), "Max Size");
                    }

                      string imagename = _imageoperation.Save(model.Image, "book");
                    
                    model.ImageUrl = imagename;
                }
                else
                {
                    if ( model.IsDeletedImg is true && !string.IsNullOrEmpty(book.ImageUrl))
                   _imageoperation.RemoveImage(book.ImageUrl , "book");
                     book.ImageUrl= null;
                }

                _mapper.Map(model, book);

                book.Categories.Clear();
                foreach (var category in model.SelectedCategories)
                {
                    book.Categories.Add(new BookCategory { CategoryId = category });
                }
                book.LastUpdatedOn = DateTime.UtcNow;
                await _bookrepo.SaveAsync();
                return RedirectToAction("Index", new { pageNumber = model.page });
            }
            model = _bookrepo.GetModel(model);
            return View("Edit", model);
        }

        public async Task< IActionResult> Delete(int id, int page) 
        {
          var book = await _bookrepo.GetByIdAsync(id);
            book.IsDeleted = !book.IsDeleted;
            await _bookrepo.SaveAsync();
            return RedirectToAction("Index", new { pageNumber = page });

        }

    }
}
