using Database.Models;

namespace Database.Interfaces
{
    public interface IResourceRepository : IRepository<Resource>
    {
        public Task<List<Resource>> GetAllActiveAsync();
        public Task<Resource?> GetActiveByIdAsync(int id);
        public Task<bool> ExistsByNameAsync(string name);
        public Task<bool> HasDependenciesAsync(int resourceId);
    }
}
