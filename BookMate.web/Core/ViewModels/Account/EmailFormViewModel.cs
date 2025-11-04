using System.ComponentModel.DataAnnotations;

namespace BookMate.web.Core.ViewModels.Account
{
    public class EmailFormViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }=string.Empty;
    }
}
