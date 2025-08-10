using Database.Models;
using Services.Services.Dto;

namespace Services.Interfaces
{
    public interface IBalanceService
    {
        public Task UpdateBalanceFromReceiptAsync(int receiptDocumentId);
        public Task RevertBalanceFromReceiptAsync(int receiptDocumentId);
        public Task UpdateBalanceFromShippingAsync(int shippingDocumentId);
        public Task RevertBalanceFromShippingAsync(int shippingDocumentId);
        public Task<bool> HasSufficientQuantity(int resourceId, int unitId, decimal requiredQuantity);
        public Task<IEnumerable<BalanceDto>> GetCurrentBalanceAsync(IEnumerable<int> resourceIds, IEnumerable<int> ueIds);
    }
}