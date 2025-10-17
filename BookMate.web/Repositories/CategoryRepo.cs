using BookMate.web.Core.Models;
using BookMate.web.Data;
using BookMate.web.Interfaces;

namespace BookMate.web.Repositories
{
    public class CategoryRepo:GenericRepo<Category>,ICategoryRepo
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepo(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }
        
        
    }
}
