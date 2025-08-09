using Database.Models;
using Services.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBalanceService
    {
        public Task UpdateBalanceFromReceiptAsync(int receiptDocumentId);
        public Task RevertBalanceFromReceiptAsync(int receiptDocumentId);
        public Task<bool> HasSufficientQuantity(int resourceId, int unitId, decimal requiredQuantity);
        public Task<IEnumerable<BalanceDto>> GetCurrentBalanceAsync();
    }
}
