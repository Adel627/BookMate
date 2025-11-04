using System.ComponentModel.DataAnnotations;

namespace BookMate.web.Core.Models
{
    public class Book:BaseModel
    {

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; }
        public string Hall { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        public int Copy {  get; set; }
        public Author Author { get; set; } = null!;
        public int AuthorId { get; set; }
        public ICollection<BookCategory> Categories { get; set; }=new List<BookCategory>();
        public ICollection<RentalBook> Rentals { get; set; } = new List<RentalBook>();
    }
}
