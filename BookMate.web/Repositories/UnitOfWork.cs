using BookMate.web.Data;
using BookMate.web.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookMate.web.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public UnitOfWork( ApplicationDbContext context,IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _environment = webHostEnvironment;
            Categories = new CategoryRepo(_context);
            Books = new BookRepo(_context , mapper);
            Authors = new AuthorRepo(_context);
            ImageOperation = new ImageOperation(_environment);

        }
        public ICategoryRepo Categories { get; private set; }
        public IBookRepo Books { get; private set; }
        public IAuthorRepo Authors { get; private set; }

        public ImageOperation ImageOperation { get; private set; }  

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

