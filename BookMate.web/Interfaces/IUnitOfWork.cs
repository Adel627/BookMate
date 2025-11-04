namespace BookMate.web.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepo Categories { get; }
        IBookRepo Books { get;}
        IAuthorRepo Authors { get; } 
        ImageOperation ImageOperation { get; }
        Task<int> CompleteAsync();

    }
}
