using System.ComponentModel.DataAnnotations;

namespace BookMate.web.Core.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        public string Code { get; set; }=string.Empty;

        public string Email {  get; set; }=string.Empty;


        [DataType(DataType.Password)]
        [RegularExpression(RegexPatterns.Password, ErrorMessage = Errors.WeakPassword)]
        public string Password { get; set; }=string.Empty;


        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [RegularExpression(RegexPatterns.Password, ErrorMessage = Errors.WeakPassword)]
        public string ConfirmPassword {  get; set; }=string.Empty;
    }
}
