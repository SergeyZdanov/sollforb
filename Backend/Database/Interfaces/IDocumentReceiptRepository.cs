using Database.Models;

namespace Database.Interfaces
{
    public interface IDocumentReceiptRepository : IRepository<DocumentReceipt>
    {
        public Task<DocumentReceipt> GetByNumberAsync(int id);
        public Task<bool> ExistsByNameAsync(int number, int? id = null);
        public Task<IEnumerable<DocumentReceipt>> GetFilteredAsync(
           DateTime? startDate,
           DateTime? endDate,
           IEnumerable<int>? documentNumbers,
           IEnumerable<int>? resourceIds,
           IEnumerable<int>? UeIds);
    }
}
