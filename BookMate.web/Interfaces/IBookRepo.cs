using BookMate.web.Core.ViewModels.Book;

namespace BookMate.web.Interfaces
{
    public interface IBookRepo:IGenericRepo<Book>
    {
        Task<PaginatedList<Book>> GetBooksAsync(string SearchTerm , int pagenumber, int pagesize);

        BookFormViewModel GetModel(BookFormViewModel? modelCreted=null);
        public IQueryable<Book> GetBook(int id);
    }
}
