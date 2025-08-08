namespace Database.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> CreateAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
