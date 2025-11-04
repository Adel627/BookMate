namespace BookMate.web.Core.ViewModels.Book
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; }
        public string Hall { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public List< string> CategoriesName { get; set; } =new List<string>();
        public int copiesCount {  get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } 
        public string? LastUpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public int page {  get; set; }
    }
}
