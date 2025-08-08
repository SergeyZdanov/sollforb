using Database.Models;

namespace Services.Interfaces
{
    public interface IUeService
    {
        public Task<UE> CreateAsync(UE resource);
        public Task<UE> GetAByIdAsync(int id);
        public Task<List<UE>> GetAllAsync(bool isActive);
        public Task UpdateAsync(int id, UE client);
        public Task DeleteAsync(int id);
        public Task<UE> GetActiveByIdAsync(int id);
    }
}
