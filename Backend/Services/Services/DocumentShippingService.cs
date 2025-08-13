using AutoMapper;
using Database.Enums;
using Database.Interfaces;
using Database.Models;
using Services.Interfaces;

namespace Services.Services
{
    public class DocumentShippingService : IDocumentShippingService
    {
        private readonly IDocumentShippingRepository _documentShippingRepository;
        private readonly IBalanceService _balanceService;
        private readonly IClientService _clientService;
        private readonly IResourceService _resourceService;
        private readonly IUeService _ueService;
        private readonly IMapper _mapper;

        public DocumentShippingService(
            IDocumentShippingRepository documentShippingRepository,
            IBalanceService balanceService,
            IClientService clientService,
            IResourceService resourceService,
            IUeService ueService,
            IMapper mapper)
        {
            _documentShippingRepository = documentShippingRepository;
            _balanceService = balanceService;
            _clientService = clientService;
            _resourceService = resourceService;
            _ueService = ueService;
            _mapper = mapper;
        }

        public async Task<DocumentShipping> CreateAsync(DocumentShipping documentShipping)
        {
            if (await ExistsByNumberAsync(documentShipping.Number))
                throw new Exception("Document number is not unique");

            if (documentShipping.ResourceShipments == null || !documentShipping.ResourceShipments.Any())
                throw new Exception("Shipping document cannot be empty");

            var client = await _clientService.GetByIdAsync(documentShipping.ClientId);
            if (client == null || client.Status == EntityStatus.Archived)
                throw new Exception("Client is not available or is in archive");

            foreach (var res in documentShipping.ResourceShipments)
            {
                var resource = await _resourceService.GetActiveByIdAsync(res.ResourceId);
                if (resource == null)
                    throw new Exception("Resource is not available or is in archive");

                var ue = await _ueService.GetActiveByIdAsync(res.UE_Id);
                if (ue == null)
                    throw new Exception("Unit of measurement is not available or is in archive");
            }

            return await _documentShippingRepository.CreateAsync(documentShipping);
        }

        public async Task UpdateAsync(int id, DocumentShipping documentShipping)
        {
            var existingDocument = await _documentShippingRepository.GetByIdAsync(id, includeResources: true);
            if (existingDocument == null)
                throw new KeyNotFoundException("Document not found");

            if (existingDocument.Status != DocumentStatus.Draft)
                throw new Exception("Only documents in draft status can be edited");

            if (documentShipping.Number != existingDocument.Number && await ExistsByNumberAsync(documentShipping.Number, id))
                throw new Exception("Document number is not unique");

            existingDocument.Number = documentShipping.Number;
            existingDocument.Date = documentShipping.Date.ToUniversalTime();
            existingDocument.ClientId = documentShipping.ClientId;

            existingDocument.ResourceShipments.Clear();

            var newResources = _mapper.Map<List<ResourceShipment>>(documentShipping.ResourceShipments);
            foreach (var resource in newResources)
            {
                existingDocument.ResourceShipments.Add(resource);
            }

            await _documentShippingRepository.UpdateAsync(existingDocument);
        }

        public async Task DeleteAsync(int id)
        {
            var document = await GetByIdAsync(id);
            if (document == null)
                throw new KeyNotFoundException("Document not found");

            if (document.Status != DocumentStatus.Draft)
                throw new Exception("Only documents in draft status can be deleted");

            await _documentShippingRepository.DeleteAsync(id);
        }

        public async Task SignAsync(int id)
        {
            var document = await _documentShippingRepository.GetByIdAsync(id, includeResources: true);
            if (document == null)
                throw new KeyNotFoundException("Document not found");

            if (document.Status != DocumentStatus.Draft)
                throw new Exception("Document is not in draft status");

            foreach (var resource in document.ResourceShipments)
            {
                if (!await _balanceService.HasSufficientQuantity(resource.ResourceId, resource.UE_Id, resource.Quantity))
                    throw new Exception($"Insufficient quantity for resource ID {resource.ResourceId}");
            }

            await _balanceService.UpdateBalanceFromShippingAsync(id);

            document.Status = DocumentStatus.Signed;
            await _documentShippingRepository.UpdateAsync(document);
        }

        public async Task RevertSignAsync(int id)
        {
            var document = await _documentShippingRepository.GetByIdAsync(id, includeResources: true);
            if (document == null)
                throw new KeyNotFoundException("Document not found");

            if (document.Status != DocumentStatus.Signed)
                throw new Exception("Document is not signed");

            await _balanceService.RevertBalanceFromShippingAsync(id);

            document.Status = DocumentStatus.Draft;
            await _documentShippingRepository.UpdateAsync(document);
        }

        public async Task<DocumentShipping> GetByIdAsync(int id, bool includeRelated = false)
        {
            return await _documentShippingRepository.GetByIdAsync(id, includeRelated);
        }

        public async Task<bool> ExistsByNumberAsync(int number, int? id = null)
        {
            return await _documentShippingRepository.ExistsByNumberAsync(number, id);
        }

        public async Task<IEnumerable<DocumentShipping>> GetFilteredAsync(
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<int>? documentNumbers,
            IEnumerable<int>? resourceIds,
            IEnumerable<int>? ueIds,
            IEnumerable<int>? clientIds)
        {
            return await _documentShippingRepository.GetFilteredAsync(startDate, endDate, documentNumbers, resourceIds, ueIds, clientIds);
        }
    }
}