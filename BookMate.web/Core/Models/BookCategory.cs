namespace BookMate.web.Core.Models
{
    public class BookCategory
    {
        public Book Book { get; set; } = default!;
        public int BookId {  get; set; }

        public Category Category { get; set; } = default!;
        public int CategoryId {  get; set; }

    }
}
