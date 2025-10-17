using BookMate.web.Core.ViewModels;
using BookMate.web.Data;
using BookMate.web.Interfaces;
using BookMate.web.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookMate.web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepo _bookrepo;
        private readonly IMapper _mapper;

        public BookController(IBookRepo bookrepo,IMapper mapper )
        {
            _bookrepo = bookrepo;
            _mapper = mapper;
            
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Create()
        {
            BookFormViewModel model = _bookrepo.GetModel();
            return View(model);
        }
        public async Task<IActionResult> SaveCreate(BookFormViewModel model)
        {
            if (ModelState.IsValid) 
            {
              var book = _mapper.Map<Book>(model);
                book.Categories = new List<BookCategory>();
                foreach (var category in model.SelectedCategories) 
                {
                   
                    book.Categories.Add(new BookCategory { CategoryId = category});
                }
               await _bookrepo.AddAsync(book);
               await  _bookrepo.SaveAsync();
                return RedirectToAction("Index");
            }
            model = _bookrepo.GetModel(model);
            return View("Create",model);
        }

        public async Task< IActionResult> Edit(int id) 
        {
            var book =  _bookrepo.GetBook(id);
            BookFormViewModel model = _mapper.Map<BookFormViewModel>(book);
            model = _bookrepo.GetModel(model);
            return View(model);
        }

    }
}
