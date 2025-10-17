namespace BookMate.web.Core.Models
{
    public class BookCopy:BaseModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = default!;
        public bool IsAvailableForRental { get; set; }
        public int EditionNumber { get; set; }
        public int SerialNumber { get; set; }
    }


}
