using BookMate.web.Data;
using BookMate.web.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookMate.web.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        

        public GenericRepo(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public async Task AddAsync(T entity)
        {
           await  _context.AddAsync(entity);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            List<T> list = await _context.Set<T>().ToListAsync();
            return list;
        }

        public async Task<T> GetByIdAsync(int id)
        {
           var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }
        public void Update(T entity)
        {
            _context.Update(entity);
            
        }
        public async Task DeleteAsync(int id)
        {
             var entity = await GetByIdAsync(id);
             _context.Remove(entity);
        }
        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }

        
    }
}
