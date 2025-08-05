using Database.Interfaces;
using Database.Models;

namespace Database.Repositoryes
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
