namespace BookMate.web.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Update(T entity);
        Task DeleteAsync(int id);
        Task SaveAsync();    


    }
}
