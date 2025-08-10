using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositoryes
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<bool> HasDependenciesAsync(int clientId)
        {
            return await Context.ShipmentDocuments.AnyAsync(d => d.ClientId == clientId);
        }
    }
}