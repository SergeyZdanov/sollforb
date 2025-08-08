using Database.Models;

namespace Services.Interfaces
{
    public interface IClientService
    {
        Task<Client> CreateAsync(Client client);
        Task<Client> GetByIdAsync(int id);
        Task<List<Client>> GetAllAsync();
        Task UpdateAsync(int id, Client client);
        Task DeleteAsync(int id);
        Task ArchiveClientAsync(int id);
    }
}
