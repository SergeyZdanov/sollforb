using Database.Enums;
using Database.Interfaces;
using Database.Models;
using Services.Interfaces;

namespace Services.Services
{
    public class DocumentReceiptService : IDocumentReceiptService
    {
        private readonly IDocumentReceiptRepository _documentReceiptRepository;

        public DocumentReceiptService(IDocumentReceiptRepository documentReceiptRepository)
        {
            _documentReceiptRepository = documentReceiptRepository;
        }
        public async Task<DocumentReceipt> CreateAsync(DocumentReceipt documentReceipt)
        {
            if (await ExistsByNameAsync(documentReceipt.Number))
                throw new Exception("Document number not unique");

            if (documentReceipt.ResourceReceipts != null && documentReceipt.ResourceReceipts.Count > 0)
            {
                foreach (var res in documentReceipt.ResourceReceipts)
                {
                    if (res.Resource.Status == EntityStatus.Archived)
                        throw new Exception("Ресурс в архиве");

                    if (res.Ue.Status == EntityStatus.Archived)
                        throw new Exception("Единица измерения в архиве");
                }
            }
            // Обновление баланса
            return await _documentReceiptRepository.CreateAsync(documentReceipt);
        }

        public async Task<DocumentReceipt> GetByIdAsync(int id)
        {
            return await _documentReceiptRepository.GetByIdAsync(id);
        }

        public async Task<List<DocumentReceipt>> GetAllAsync(bool isActive)
        {
            return await _documentReceiptRepository.GetAllAsync();
        }


        public async Task UpdateAsync(int id, DocumentReceipt documentReceipt)
        {
            if (documentReceipt.Id != id)
                throw new Exception("Id не совпадают");

            var result = await GetByIdAsync(id);

            if (documentReceipt.Number != result.Number || await ExistsByNameAsync(documentReceipt.Number))
                throw new Exception("Document number not unique");

            documentReceipt.Id = id;

            await _documentReceiptRepository.UpdateAsync(documentReceipt);
        }

        public async Task DeleteAsync(int id)
        {
            await _documentReceiptRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(int number, int? id = null)//будут ли у меня новые документы с id
        {
            if (id.HasValue)
                return await _documentReceiptRepository.ExistsByNameAsync(number, id);

            return await _documentReceiptRepository.ExistsByNameAsync(number);
        }

        public async Task<IEnumerable<DocumentReceipt>> GetFilteredAsync(DateTime? startDate, DateTime? endDate, IEnumerable<int>? documentNumbers, IEnumerable<int>? resourceIds, IEnumerable<int>? UeIds)
        {
            return await _documentReceiptRepository.GetFilteredAsync(startDate, endDate, documentNumbers, resourceIds, UeIds);
        }
    }
}
