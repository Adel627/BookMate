namespace BookMate.web.Core.Models
{
    public class Category:BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public ICollection<BookCategory> Books { get; set; } =new List<BookCategory>();

    }
}
