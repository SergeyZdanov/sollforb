using Database.Models;

namespace Database.Interfaces
{
    public interface IDocumentShippingRepository : IRepository<DocumentShipping>
    {
        public Task<DocumentShipping> GetByIdAsync(int id, bool includeResources = false);
        public Task<bool> ExistsByNumberAsync(int number, int? id = null);
        public Task<IEnumerable<DocumentShipping>> GetFilteredAsync(
           DateTime? startDate,
           DateTime? endDate,
           IEnumerable<int>? documentNumbers,
           IEnumerable<int>? resourceIds,
           IEnumerable<int>? ueIds,
           IEnumerable<int>? clientIds);
    }
}
