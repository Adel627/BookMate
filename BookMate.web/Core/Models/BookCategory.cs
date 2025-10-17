namespace BookMate.web.Core.Models
{
    public class BookCategory
    {
        public Book Book { get; set; } = new Book();
        public int BookId {  get; set; }

        public Category Category { get; set; }=new Category();
        public int CategoryId {  get; set; }

    }
}
