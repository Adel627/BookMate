using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookMate.web.Core.ViewModels.User
{
    public class AddUserViewModel
    {
        public string FullName { get; set; } = null!;


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }=string.Empty;


        [RegularExpression(RegexPatterns.Username , ErrorMessage =Errors.InvalidUsername)]
        public string UserName { get; set; } = string.Empty;


        [DataType(DataType.Password)]
        [RegularExpression(RegexPatterns.Password,ErrorMessage =Errors.WeakPassword)]
        public string Password { get; set; } = string.Empty;


        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [RegularExpression(RegexPatterns.Password , ErrorMessage =Errors.WeakPassword)]
        public string ConfirmPassword { get; set; } = string.Empty;


        public IEnumerable<SelectListItem>? Roles { get; set; }= new List<SelectListItem>();
        
        
        [Display(Name ="Roles")]
        public IList<string> SelectedRole {  get; set; } = new List<string>();       
        public bool ShowRoleSelection { get; set; } // to show dropdown list of roles or not
    }
}
