using BookMate.web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Text;
using System.Text.Encodings.Web;

namespace BookMate.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IEmailBodyBuilder _emailBodyBuilder;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender
             , IEmailBodyBuilder emailBodyBuilder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _emailBodyBuilder = emailBodyBuilder;
        }

        public IActionResult Index()
        {

            return Content("Index of Account");
        }
        public IActionResult Register(string role)
        {
            AddUserViewModel Vm= new AddUserViewModel();
            Vm.SelectedRole.Add(role);
            return View("_CreateUserPartial",Vm);
        }
        public async Task<IActionResult> SaveUser(AddUserViewModel model) 
        {

            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    FullName = model.FullName,
                };
                IdentityResult result =
                   await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    IdentityResult Result = await _userManager.AddToRolesAsync(appUser, model.SelectedRole);
                    if (Result.Succeeded)
                    {
                        //Send Message
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Action(   
                            "ConfirmEmail",
                            "Account",
                            values: new { userId = appUser.Id, code },
                           Request.Scheme

                            );


                        var placeholders = new Dictionary<string, string>()
                        {             
                             { "imageUrl", "https://res.cloudinary.com/ddkthlwge/image/upload/v1762219319/Email_pemcv8.jpg" },
                             { "header", $"Hey {appUser.FullName}, thanks for joining us!" },
                             { "body", "please confirm your email" },
                             {"url" ,  $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
                            {"linkTitle" , "Active Account!" }
                        };

                        var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Email,placeholders);
                        await _emailSender.SendEmailAsync(appUser.Email, "Confirm Your Account", body  );
                        return View("RegisterConfirmation");
                    }
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View("_CreateUserPartial", model);
        }
        public async Task< IActionResult> ConfirmEmail(string userId , string code)
        {
            if (userId == null || code == null)
                return BadRequest("The Request To Confirm Email Invalid");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User Not Found");
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

           string StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            return View("ConfirmEmail",StatusMessage);


            
        }
        public IActionResult Login()
        {
            return View();
        }
        public async Task< IActionResult> SaveLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appuser =  await _userManager.Users.SingleOrDefaultAsync
                        (u => (u.Email == model.UserName || u.UserName == model.UserName)&&u.IsDeleted==false);
                
                if (appuser!= null) 
                {
                  var result = await _signInManager.PasswordSignInAsync
                        (appuser,model.Password, isPersistent: model.RememberMe ,lockoutOnFailure : true );
                    if (result.Succeeded) 
                      return RedirectToAction("Index","Home"); 
                    
                    if (result.IsLockedOut)
                      return View("Lockout");

                    if (result.IsNotAllowed)
                    {
                        EmailFormViewModel email = new() { Email = appuser.Email };
                        return View("ResendEmailConfirmation",email );
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View("Login");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View("Login" , model);

        }
        
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> ForgotPassword(EmailFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null) 
            {
              var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    values: new { userId = user.Id, code },
                    Request.Scheme

                    );
                var placeholders = new Dictionary<string, string>()
                        {
                             { "imageUrl", "https://res.cloudinary.com/ddkthlwge/image/upload/v1762297054/computer-security-with-login-password-padlock_rtq1t0.jpg" },
                             { "header", $"Hey {user.FullName}, Reset Password!" },
                             { "body", "please reset your password" },
                             {"url" ,   $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
                            {"linkTitle" ,"Reset Password!" }
                        };

                var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Email , placeholders );
                await _emailSender.SendEmailAsync(user.Email!, "Reset Your Password", body);
            }
            return View("ForgotPasswordConfirmation");


        }
        public async Task< IActionResult> ResetPassword(string userId , string code) 
        {
            if (userId == null || code == null)
                return BadRequest("Invalid Request");
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null) 
            {
                ResetPasswordViewModel Vm = new() { Code = code , Email=user.Email! };
                return View(Vm);
            }
            return NotFound("User Not Found");
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null) 
            {
                model.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
              var result = await _userManager.ResetPasswordAsync(user,model.Code,model.Password);
                if (result.Succeeded)
                {
                    return View("ResetPasswordConfirmation");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(model);

                }

            }
            return View("ResetPasswordConfirmation");

        }
        public async Task< IActionResult> ResendEmailConfirmation(EmailFormViewModel model)
        {
            if (model.Email == string.Empty)
                return View();

            if(ModelState.IsValid) 
            {
                
              var user = await _userManager.FindByEmailAsync (model.Email);
                if (user != null && user.EmailConfirmed is false) 
                {
                    //Send Message
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        values: new { userId = user.Id, code },
                        Request.Scheme

                        );
                    var placeholders = new Dictionary<string, string>()
                        {
                             { "imageUrl", "https://res.cloudinary.com/ddkthlwge/image/upload/v1762219319/Email_pemcv8.jpg" },
                             { "header",    $"Hey {user.FullName}, We Resend Email To You!"},
                             { "body", "please confirm your email" },
                             {"url" ,   $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
                            {"linkTitle" ,"Active Account!"}
                        };


                    var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Email , placeholders);
                    await _emailSender.SendEmailAsync(user.Email!, "Confirm Your Account", body);
                }
                return View("RegisterConfirmation");

            }
            ModelState.AddModelError("", "Invalid Email");
            return View(model);
        }
        public async Task< IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index" , "Home");
        }

    }
}
