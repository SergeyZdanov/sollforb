using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDocumentShippingService
    {
        public Task<DocumentShipping> CreateAsync(DocumentShipping documentShipping);
        public Task<DocumentShipping> GetByIdAsync(int id, bool includeRelated = false);
        public Task UpdateAsync(int id, DocumentShipping documentShipping);
        public Task DeleteAsync(int id);
        public Task<bool> ExistsByNumberAsync(int number, int? id = null);
        public Task<IEnumerable<DocumentShipping>> GetFilteredAsync(
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<int>? documentNumbers,
            IEnumerable<int>? resourceIds,
            IEnumerable<int>? ueIds,
            IEnumerable<int>? clientIds);
        public Task SignAsync(int id);
        public Task RevertSignAsync(int id);
    }
}
