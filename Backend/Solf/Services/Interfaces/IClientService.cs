using Database.Models;

namespace Services.Interfaces
{
    public interface IClientService
    {
        Task<List<Client>> GetAllAsync();
    }
}
