using BookMate.web.Extensions;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookMate.web.Controllers
{
    [Authorize(Roles =AppRoles.Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager ,RoleManager<IdentityRole> roleManager , IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return Content("Index");
        }
        public async Task< IActionResult> AddUser()
        {

            var roles = await _roleManager.Roles.ToListAsync();
            AddUserViewModel vm = new AddUserViewModel()
            {
                Roles = _mapper.Map<IEnumerable<SelectListItem>>(roles),
                ShowRoleSelection = true
            };
            
            return View("_CreateUserPartial", vm);
        }
        public async Task<IActionResult> SaveUser(AddUserViewModel model)
        {

            if (ModelState.IsValid) 
            {
                ApplicationUser appUser = new ApplicationUser()
                {
                    Email = model.Email,
                    EmailConfirmed=true,
                    UserName = model.UserName,
                    FullName = model.FullName,
                    CreatedById = User.GetUserId(), 
                };
                IdentityResult result =
                   await _userManager.CreateAsync(appUser, model.Password);
                
                if (result.Succeeded) 
                {
                    IdentityResult Result = await _userManager.AddToRolesAsync(appUser, model.SelectedRole);
                    if (Result.Succeeded) 
                    {
                      return RedirectToAction("Index");
                    }
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                foreach (var error in result.Errors) 
                {
                  ModelState.AddModelError("",error.Description);
                }
                
            }
            var roles = await _roleManager.Roles.ToListAsync();
            model.Roles = _mapper.Map<IEnumerable<SelectListItem>>(roles);
            return View("_CreateUserPartial", model);
        }
    }
}
