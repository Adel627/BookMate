using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookMate.web.Core.ViewModels.Book
{
    public class BookFormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsDeletedImg { get; set; }  
        public string? ImageUrl { get; set; }
        public string Hall { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        public IEnumerable<SelectListItem>? Authors { get; set; } = default!;
        [Display(Name = "Authors")]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; } = default!;

        [Display(Name = "Categories")]
        public IList<int> SelectedCategories { get; set; } = new List<int>();
        public int Copy { get; set; }

        public int page { get; set; }

    }
}
