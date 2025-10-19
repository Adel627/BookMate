using BookMate.web.Core.ViewModels.Book;
using BookMate.web.Data;
using BookMate.web.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookMate.web.Repositories
{
    public class BookRepo:GenericRepo<Book>,IBookRepo
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public BookRepo(ApplicationDbContext context,IMapper mapper):base(context) 
        {
            _context = context;
            _mapper = mapper;
            
        }

        public async Task<PaginatedList<Book>> GetBooksAsync(string SearchTerm , int pagenumber , int pagesize) 
        {
          IQueryable<Book> query =  _context.Books;
            query = !string.IsNullOrEmpty(SearchTerm) ? 
                query.Where( b=> b.Title.Contains(SearchTerm) ) : query;

            PaginatedList<Book> books = await PaginatedList<Book>
                .CreateAsync(query, pagenumber, pagesize);
            return books;
        }
        public BookFormViewModel GetModel(BookFormViewModel? modelCreted=null)
        {
            IEnumerable<Author> authors = _context.Author;
            IEnumerable<Category> categories = _context.Categories;
            if (modelCreted == null) 
            {
                BookFormViewModel model = new BookFormViewModel()
                {
                    Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors),
                    Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories),
                };
                return model;
            }
            modelCreted.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            modelCreted.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            return modelCreted;

        }
        public IQueryable< Book> GetBook(int id) 
        {
            var book = _context.Books.Include(x => x.Categories)
                .Include(b => b.Author)
                .Where(x => x.Id ==id);
            return book;
        }


    }
}
