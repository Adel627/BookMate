using ExpressiveAnnotations.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookMate.web.Core.ViewModels.Subscriber
{
    public class SubscriberFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100), Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;


        [MaxLength(100), Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [AssertThat("DateOfBirth <= Today()", ErrorMessage = Errors.NotAllowFutureDates)]
        [Display(Name ="Date Of Birth")]
        public DateTime DateOfBirth { get; set; }


        [MaxLength(14), Display(Name = "National ID"),
            RegularExpression(RegexPatterns.NationalId, ErrorMessage = Errors.InvalidNationalId)]
        [Remote("AllowNationalId", null!, ErrorMessage = Errors.Duplicated)]

        public string NationalId { get; set; } = null!;

        [MaxLength(11) , Display(Name ="Mobile Number")]
        [RegularExpression(RegexPatterns.MobileNumber ,ErrorMessage =Errors.InvalidMobileNumber)]
        [Remote("AllowMobileNumber", null!, ErrorMessage = Errors.Duplicated)]

        public string MobileNumber { get; set; } = null!;

        [Display(Name ="Has Whatsapp")]
        public bool HasWhatsApp { get; set; }

        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        [Remote("AllowEmail", null!, ErrorMessage = Errors.Duplicated)]
        public string Email { get; set; } = null!;

        public IFormFile Image { get; set; }=default!;

        [Display(Name = "Governorate")]
        public int GovernorateId { get; set; }
        public IEnumerable<SelectListItem>? Governorates { get; set; }


        [Display(Name ="Area")]
        public int AreaId { get; set; }
        public IEnumerable<SelectListItem>? Areas { get; set; } = new List<SelectListItem>();


        [MaxLength(500)]
        public string Address { get; set; } = null!;

        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
    }
}
