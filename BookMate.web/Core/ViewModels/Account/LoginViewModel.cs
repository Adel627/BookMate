using System.ComponentModel.DataAnnotations;

namespace BookMate.web.Core.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name ="Email / UserName")]
        public string UserName {  get; set; } = string.Empty;

        [RegularExpression(RegexPatterns.Password,ErrorMessage =Errors.WeakPassword)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name ="Remeber Me!!")]
        public bool RememberMe { get; set; }
    }
}
