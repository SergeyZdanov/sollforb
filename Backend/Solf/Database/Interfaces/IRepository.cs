namespace Database.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
    }
}
