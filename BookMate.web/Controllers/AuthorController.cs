using BookMate.web.Interfaces;
using BookMate.web.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace BookMate.web.Controllers
{
    public class AuthorController : Controller
    {
        
        private readonly IMapper _mapper;
        private readonly IGenericRepo<Author> _repo;

        public AuthorController(IMapper mapper,IGenericRepo<Author> repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task< IActionResult> Index()
        {
            var authors = await _repo.GetAllAsync();
            return View(authors);
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> SaveCreate(AuthorFormViewModel model)
        {
            if (ModelState.IsValid) 
            {
              var author = _mapper.Map<Author>(model);
               await  _repo.AddAsync(author);
               await _repo.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Create",model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Author author = await _repo.GetByIdAsync(id);
            AuthorFormViewModel model = _mapper.Map<AuthorFormViewModel>(author);

            return View(model);
        }

        public async Task<IActionResult> SaveEdit(AuthorFormViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                Author author = await _repo.GetByIdAsync(id);
                author.Name = model.Name;
                author.LastUpdatedOn = DateTime.UtcNow;
                await _repo.SaveAsync();
                _repo.Update(author);
                return RedirectToAction("Index");
            }
            return View("Edit", id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveAsync();
            return RedirectToAction("Index");
        }


    }
}
