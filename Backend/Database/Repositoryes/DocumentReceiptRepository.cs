using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Database.Repositoryes
{
    public class DocumentReceiptRepository : BaseRepository<DocumentReceipt>, IDocumentReceiptRepository
    {
        public DocumentReceiptRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<DocumentReceipt> GetByIdAsync(int id, bool includeResources = false)
        {
            var query = Context.ReceiptDocuments.AsQueryable();

            if (includeResources) 
            {
                query.Include(d => d.ResourceReceipts)
                    .ThenInclude(e => e.Resource)
                    .Include(x => x.ResourceReceipts)
                    .ThenInclude(r => r.Ue);
            }

            return await query.FirstOrDefaultAsync(d => d.Id == id);
        }
                


        public async override Task DeleteAsync(int id)
        {
            var document = await Context.ReceiptDocuments
                .Include(d => d.ResourceReceipts)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (document != null)
            {
                Context.ReceiptDocuments.Remove(document);
                await Context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByNameAsync(int number, int? id = null)
        {
            var query = Context.ReceiptDocuments.AsQueryable();
            if (id.HasValue)
                query.AsQueryable().Where(r => r.Id != id.Value);

            return await query.AnyAsync(r => r.Number == number);
        }

        public async Task<DocumentReceipt> GetByNumberAsync(int number)
        {
            return await EntitySet.FirstOrDefaultAsync(d => d.Number == number);
        }

        public async Task<IEnumerable<DocumentReceipt>> GetFilteredAsync(
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<int>? documentNumbers,
            IEnumerable<int>? resourceIds,
            IEnumerable<int>? UeIds)
        {
            var query = Context.ReceiptDocuments
                .Include(d => d.ResourceReceipts)
                .ThenInclude(r => r.Resource)
                .Include(d => d.ResourceReceipts)
                .ThenInclude(r => r.Ue)
                .AsQueryable();


            if (startDate.HasValue)
            {
                query = query.Where(d => d.Date >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(d => d.Date <= endDate.Value);
            }

            if (documentNumbers != null && documentNumbers.Any())
            {
                query = query.Where(d => documentNumbers.Contains(d.Number));
            }

            if (resourceIds != null && resourceIds.Any())
            {
                query = query.Where(d => d.ResourceReceipts
                    .Any(r => resourceIds.Contains(r.ResourceId)));
            }

            if (UeIds != null && UeIds.Any())
            {
                query = query.Where(d => d.ResourceReceipts
                    .Any(r => UeIds.Contains(r.UE_Id)));
            }

            return await query.ToListAsync();
        }
    }
}
