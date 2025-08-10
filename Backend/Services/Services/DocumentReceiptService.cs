using AutoMapper;
using Database.Enums;
using Database.Interfaces;
using Database.Models;
using Services.Interfaces;

namespace Services.Services
{
    public class DocumentReceiptService : IDocumentReceiptService
    {
        private readonly IDocumentReceiptRepository _documentReceiptRepository;
        private readonly IBalanceService _balanceService;
        private readonly IResourceService _resourceService;
        private readonly IUeService _ueService;
        private readonly IMapper _mapper;

        public DocumentReceiptService(
            IDocumentReceiptRepository documentReceiptRepository,
            IBalanceService balanceService,
            IResourceService resourceService,
            IUeService ueService,
            IMapper mapper)
        {
            _documentReceiptRepository = documentReceiptRepository;
            _balanceService = balanceService;
            _resourceService = resourceService;
            _ueService = ueService;
            _mapper = mapper;
        }

        public async Task<DocumentReceipt> CreateAsync(DocumentReceipt documentReceipt)
        {
            if (await ExistsByNameAsync(documentReceipt.Number))
                throw new Exception("Document number not unique");

            if (documentReceipt.ResourceReceipts != null)
            {
                foreach (var res in documentReceipt.ResourceReceipts)
                {
                    var resource = await _resourceService.GetActiveByIdAsync(res.ResourceId);
                    if (resource == null)
                        throw new Exception("Resource is not available or is in archive");

                    var ue = await _ueService.GetActiveByIdAsync(res.UE_Id);
                    if (ue == null)
                        throw new Exception("Unit of measurement is not available or is in archive");
                }
            }

            var createdDocument = await _documentReceiptRepository.CreateAsync(documentReceipt);

            await _balanceService.UpdateBalanceFromReceiptAsync(createdDocument.Id);

            return createdDocument;
        }

        public async Task UpdateAsync(int id, DocumentReceipt documentReceipt)
        {
            var existingDocument = await _documentReceiptRepository.GetByIdAsync(id, includeResources: true);
            if (existingDocument == null)
                throw new KeyNotFoundException("Document not found");

            if (documentReceipt.Number != existingDocument.Number && await ExistsByNameAsync(documentReceipt.Number, id))
                throw new Exception("Document number is not unique");

            await _balanceService.RevertBalanceFromReceiptAsync(id);

            existingDocument.Number = documentReceipt.Number;
            existingDocument.Date = documentReceipt.Date;
            existingDocument.ResourceReceipts.Clear();
            var newResources = _mapper.Map<List<ResourceReceipt>>(documentReceipt.ResourceReceipts);
            foreach (var resource in newResources)
            {
                existingDocument.ResourceReceipts.Add(resource);
            }
            await _documentReceiptRepository.UpdateAsync(existingDocument);

            await _balanceService.UpdateBalanceFromReceiptAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _balanceService.RevertBalanceFromReceiptAsync(id);
            await _documentReceiptRepository.DeleteAsync(id);
        }

        public async Task<DocumentReceipt> GetByIdAsync(int id)
        {
            return await _documentReceiptRepository.GetByIdAsync(id, includeResources: true);
        }

        public async Task<List<DocumentReceipt>> GetAllAsync(bool isActive)
        {
            return await _documentReceiptRepository.GetAllAsync();
        }

        public async Task<bool> ExistsByNameAsync(int number, int? id = null)
        {
            return await _documentReceiptRepository.ExistsByNameAsync(number, id);
        }

        public async Task<IEnumerable<DocumentReceipt>> GetFilteredAsync(DateTime? startDate, DateTime? endDate, IEnumerable<int>? documentNumbers, IEnumerable<int>? resourceIds, IEnumerable<int>? UeIds)
        {
            return await _documentReceiptRepository.GetFilteredAsync(startDate, endDate, documentNumbers, resourceIds, UeIds);
        }
    }
}