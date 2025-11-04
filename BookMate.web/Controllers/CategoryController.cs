using BookMate.web.Extensions;
using BookMate.web.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMate.web.Controllers
{
    [Authorize(Roles =AppRoles.Archive)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitofwork = unitOfWork;
            _mapper = mapper;      
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _unitofwork.Categories.GetAllAsync();   
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
                category.CreatedById = User.GetUserId();
               await _unitofwork.Categories.AddAsync(category);
               await _unitofwork.CompleteAsync();
                return RedirectToAction("Index");
            }
            return View("Create",model);
            
        }

        public async Task<IActionResult> Edit(int id) 
        {
            Category category = await _unitofwork.Categories.GetByIdAsync(id);
            categoryFormViewModel categoryFormViewModel = _mapper.Map<categoryFormViewModel>(category);
               
            return View(categoryFormViewModel);
        }

        public async Task<IActionResult> SaveEdit(categoryFormViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                Category category = await _unitofwork.Categories.GetByIdAsync(id);
                category.Name = model.Name;
                category.LastUpdatedOn = DateTime.UtcNow;
                category.LastUpdatedById = User.GetUserId();
                await _unitofwork.CompleteAsync();
                _unitofwork.Categories.Update(category);
                return RedirectToAction("Index");
            }
            return View("Edit", id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _unitofwork.Categories.GetByIdAsync(id);
            author.IsDeleted = !author.IsDeleted;
            await _unitofwork.CompleteAsync();
            return RedirectToAction("Index");
        }

    }
}
