using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IBalanceRepository : IRepository<Balance>
    {
        Task<Balance> GetByResourceAndUnitAsync(int resourceId, int ue_id);
    }
}
