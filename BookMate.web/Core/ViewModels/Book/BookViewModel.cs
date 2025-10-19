namespace BookMate.web.Core.ViewModels.Book
{
    public class BookViewModel
    {
        public PaginatedList<Models.Book> Books { get; set; } = default!;
        public string SearchTerm {  get; set; } =string.Empty;
        public int PageSize { get; set; }   
    }
}
