using BookMate.web.Data;
using BookMate.web.Interfaces;

namespace BookMate.web.Repositories
{
    public class AuthorRepo:GenericRepo<Author>,IAuthorRepo
    {
        private readonly ApplicationDbContext _context;
        public AuthorRepo(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

    }
}
