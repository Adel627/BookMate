using BookMate.web.Core.ViewModels;

namespace BookMate.web.Interfaces
{
    public interface IBookRepo:IGenericRepo<Book>
    {
        BookFormViewModel GetModel(BookFormViewModel? modelCreted=null);
        public IQueryable<Book> GetBook(int id);
    }
}
