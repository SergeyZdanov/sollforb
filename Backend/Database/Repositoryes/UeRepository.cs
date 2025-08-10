using Database.Enums;
using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositoryes
{
    public class UeRepository : BaseRepository<UE>, IUeRepository
    {
        public UeRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await Context.Ue.AnyAsync(r => r.Name == name);
        }

        public async Task<UE?> GetActiveByIdAsync(int id)
        {
            return await Context.Ue
                .Where(r => r.Id == id && r.Status == EntityStatus.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<List<UE>> GetAllActiveAsync()
        {
            return await Context.Ue.Where(r => r.Status == EntityStatus.Active).ToListAsync();
        }

        public async Task<bool> HasDependenciesAsync(int UeId)
        {
            bool hasBalance = await Context.Balances.AnyAsync(b => b.UE_Id == UeId);
            bool hasReceipts = await Context.ReceiptResources.AnyAsync(r => r.UE_Id == UeId);
            bool hasShipments = await Context.ShipmentResources.AnyAsync(s => s.UE_Id == UeId);


            return hasBalance || hasReceipts || hasShipments;
        }
    }
}
