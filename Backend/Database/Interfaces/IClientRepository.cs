using Database.Models;

namespace Database.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<bool> HasDependenciesAsync(int clientId);
    }
}
