using BookMate.web.Extensions;
using BookMate.web.Interfaces;
using BookMate.web.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMate.web.Controllers
{
    [Authorize(Roles = AppRoles.Archive)]
    public class AuthorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task< IActionResult> Index()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();
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
                author.CreatedById = User.GetUserId();
               await  _unitOfWork.Authors.AddAsync(author);
               await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            return View("Create",model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Author author = await _unitOfWork.Authors.GetByIdAsync(id);
            AuthorFormViewModel model = _mapper.Map<AuthorFormViewModel>(author);

            return View(model);
        }

        public async Task<IActionResult> SaveEdit(AuthorFormViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                Author author = await _unitOfWork.Authors.GetByIdAsync(id);
                author.Name = model.Name;
                author.LastUpdatedOn = DateTime.UtcNow;
                author.LastUpdatedById = User.GetUserId();
                await _unitOfWork.CompleteAsync();
                _unitOfWork.Authors.Update(author);
                return RedirectToAction("Index");
            }
            return View("Edit", id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            author.IsDeleted = !author.IsDeleted;
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }


    }
}
