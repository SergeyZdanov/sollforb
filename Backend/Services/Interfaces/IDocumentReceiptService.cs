using Database.Models;

namespace Services.Interfaces
{
    public interface IDocumentReceiptService
    {
        public Task<DocumentReceipt> CreateAsync(DocumentReceipt documentReceipt);
        public Task<DocumentReceipt> GetByIdAsync(int id);
        public Task<List<DocumentReceipt>> GetAllAsync(bool isActive);
        public Task UpdateAsync(int id, DocumentReceipt documentReceipt);
        public Task DeleteAsync(int id);
        public Task<bool> ExistsByNameAsync(int number, int? id = null);
        public Task<IEnumerable<DocumentReceipt>> GetFilteredAsync(
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<int>? documentNumbers,
            IEnumerable<int>? resourceIds,
            IEnumerable<int>? UeIds);
    }
}
