using BookMate.web.Core.Enums;

namespace BookMate.web.Core.Models
{
    public class RentalBook
    {
        public int RentalId { get; set; }
        public Rental? Rental { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public DateTime RentalDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays((int)RentalsConfigurations.RentalDuration);
        public DateTime? ReturnDate { get; set; }
        public DateTime? ExtendedOn { get; set; }
    }
}
