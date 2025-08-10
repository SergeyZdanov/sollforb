using Database.Enums;
using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositoryes
{
    public class ResourceRepository : BaseRepository<Resource>, IResourceRepository
    {
        public ResourceRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await Context.Resources.AnyAsync(r => r.Name == name);
        }

        public async Task<Resource?> GetActiveByIdAsync(int id)
        {
            return await Context.Resources
                .Where(r => r.Id == id && r.Status == EntityStatus.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Resource>> GetAllActiveAsync()
        {
            return await Context.Resources.Where(r => r.Status == EntityStatus.Active).ToListAsync();
        }

        public async Task<bool> HasDependenciesAsync(int resourceId)
        {
            bool hasBalance = await Context.Balances.AnyAsync(b => b.ResourceId == resourceId);
            bool hasReceipts = await Context.ReceiptResources.AnyAsync(r => r.ResourceId == resourceId);
            bool hasShipments = await Context.ShipmentResources.AnyAsync(s => s.ResourceId == resourceId);

            return hasBalance || hasReceipts || hasShipments;
        }
    }
}
