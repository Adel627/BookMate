using BookMate.web.Core.ViewModels;
using BookMate.web.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace BookMate.web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _categoryrepo;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepo _categoryRepo , IMapper mapper)
        {
            _categoryrepo = _categoryRepo;
            _mapper = mapper;

            
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryrepo.GetAllAsync();   
            return View(categories);
        }

        public IActionResult Create() 
        {
            return View();
        }

        public async Task<IActionResult> SaveCreate(categoryFormViewModel model)
        {
            if (ModelState.IsValid) 
            {
                Category category = _mapper.Map<Category>(model); 
               await _categoryrepo.AddAsync(category);
               await _categoryrepo.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Create",model);
            
        }

        public async Task<IActionResult> Edit(int id) 
        {
            Category category = await _categoryrepo.GetByIdAsync(id);
            categoryFormViewModel categoryFormViewModel = _mapper.Map<categoryFormViewModel>(category);
               
            return View(categoryFormViewModel);
        }

        public async Task<IActionResult> SaveEdit(categoryFormViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                Category category = await _categoryrepo.GetByIdAsync(id);
                category.Name = model.Name;
                category.LastUpdatedOn = DateTime.UtcNow;
                await _categoryrepo.SaveAsync();
                _categoryrepo.Update(category);
                return RedirectToAction("Index");
            }
            return View("Edit", id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryrepo.DeleteAsync(id);
            await _categoryrepo.SaveAsync();
            return RedirectToAction("Index");
        } 

    }
}
