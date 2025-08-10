using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositoryes
{
    public class BalanceRepository : BaseRepository<Balance>, IBalanceRepository
    {
        public BalanceRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<Balance> GetByResourceAndUnitAsync(int resourceId, int ue_id)
        {
            return await Context.Balances
                .Include(b => b.Resource)
                .Include(b => b.Ue)
                .FirstOrDefaultAsync(b =>
                    b.ResourceId == resourceId &&
                    b.UE_Id == ue_id);
        }

        public override async Task<List<Balance>> GetAllAsync()
        {
            return await Context.Balances
                .Include(b => b.Resource)
                .Include(b => b.Ue)
                .ToListAsync();
        }

        public async Task<IEnumerable<Balance>> GetFilteredAsync(IEnumerable<int> resourceIds, IEnumerable<int> ueIds)
        {
            var query = Context.Balances
                .Include(b => b.Resource)
                .Include(b => b.Ue)
                .AsQueryable();

            if (resourceIds != null && resourceIds.Any())
            {
                query = query.Where(b => resourceIds.Contains(b.ResourceId));
            }

            if (ueIds != null && ueIds.Any())
            {
                query = query.Where(b => ueIds.Contains(b.UE_Id));
            }

            return await query.ToListAsync();
        }
    }
}