using Database.Models;

namespace Database.Interfaces
{
    public interface IUeRepository : IRepository<UE>
    {
        public Task<List<UE>> GetAllActiveAsync();
        public Task<UE?> GetActiveByIdAsync(int id);
        public Task<bool> ExistsByNameAsync(string name);
        public Task<bool> HasDependenciesAsync(int resourceId);
    }
}
