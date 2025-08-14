using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositoryes
{
    public class DocumentShippingRepository : BaseRepository<DocumentShipping>, IDocumentShippingRepository
    {
        public DocumentShippingRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<DocumentShipping> GetByIdAsync(int id, bool includeResources = false)
        {
            var query = Context.ShipmentDocuments.AsQueryable();

            if (includeResources)
            {
                query = query.Include(d => d.ResourceShipments)
                    .ThenInclude(e => e.Resource)
                    .Include(x => x.ResourceShipments)
                    .ThenInclude(r => r.Ue);
            }

            return await query.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async override Task DeleteAsync(int id)
        {
            var document = await Context.ShipmentDocuments
                .Include(d => d.ResourceShipments)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (document != null)
            {
                Context.ShipmentDocuments.Remove(document);
                await Context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByNumberAsync(int number, int? id = null)
        {
            var query = Context.ShipmentDocuments.AsQueryable();
            if (id.HasValue)
            {
                query = query.Where(r => r.Id != id.Value);
            }

            return await query.AnyAsync(r => r.Number == number);
        }

        public async Task<IEnumerable<DocumentShipping>> GetFilteredAsync(
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<int>? documentNumbers,
            IEnumerable<int>? resourceIds,
            IEnumerable<int>? ueIds,
            IEnumerable<int>? clientIds)
        {
            var query = Context.ShipmentDocuments
                .Include(d => d.Client)
                .Include(d => d.ResourceShipments)
                .ThenInclude(r => r.Resource)
                .Include(d => d.ResourceShipments)
                .ThenInclude(r => r.Ue)
                .AsQueryable();

            if (startDate.HasValue)
            {
                var utcStartDate = startDate.Value.ToUniversalTime();
                query = query.Where(d => d.Date >= utcStartDate);
            }
            if (endDate.HasValue)
            {
                var utcEndDate = endDate.Value.ToUniversalTime();
                query = query.Where(d => d.Date <= utcEndDate);
            }

            if (documentNumbers != null && documentNumbers.Any())
            {
                query = query.Where(d => documentNumbers.Contains(d.Number));
            }

            if (resourceIds != null && resourceIds.Any())
            {
                query = query.Where(d => d.ResourceShipments
                    .Any(r => resourceIds.Contains(r.ResourceId)));
            }

            if (ueIds != null && ueIds.Any())
            {
                query = query.Where(d => d.ResourceShipments
                    .Any(r => ueIds.Contains(r.UE_Id)));
            }

            if (clientIds != null && clientIds.Any())
            {
                query = query.Where(d => clientIds.Contains(d.ClientId));
            }

            return await query.ToListAsync();
        }
    }
}