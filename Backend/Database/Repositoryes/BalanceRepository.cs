using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
