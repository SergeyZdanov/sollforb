using Database.Models;

namespace Services.Interfaces
{
    public interface IResourceService
    {
        public Task<Resource> CreateAsync(Resource resource);
        public Task<Resource> GetByIdAsync(int id);
        public Task<List<Resource>> GetAllAsync(bool isActive);
        public Task UpdateAsync(int id, Resource client);
        public Task DeleteAsync(int id);
        public Task<Resource> GetActiveByIdAsync(int id);
    }
}
