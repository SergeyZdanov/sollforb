using Database.Interfaces;
using Database.Models;
using Services.Interfaces;
using Services.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IDocumentReceiptRepository _receiptRepository;

        public BalanceService(IBalanceRepository balanceRepository, IDocumentReceiptRepository receiptRepository)
        {
            _balanceRepository = balanceRepository;
            _receiptRepository = receiptRepository;
        }

        public async Task UpdateBalanceFromReceiptAsync(int receiptDocumentId)
        {
            var document = await _receiptRepository.GetByIdAsync(receiptDocumentId, includeResources: true)
                ?? throw new KeyNotFoundException("Receipt document not found");

            foreach (var resource in document.ResourceReceipts)
            {
                await AdjustBalanceAsync(
                    resource.ResourceId,
                    resource.UE_Id,
                    resource.Quantity,
                    isIncrease: true);
            }
        }

        public async Task RevertBalanceFromReceiptAsync(int receiptDocumentId)
        {
            var document = await _receiptRepository.GetByIdAsync(receiptDocumentId, includeResources: true)
                ?? throw new KeyNotFoundException("Receipt document not found");

            foreach (var resource in document.ResourceReceipts)
            {
                await AdjustBalanceAsync(
                    resource.ResourceId,
                    resource.UE_Id,
                    resource.Quantity,
                    isIncrease: false);
            }
        }

        public async Task<bool> HasSufficientQuantity(int resourceId, int unitId, decimal requiredQuantity)
        {
            var balance = await _balanceRepository.GetByResourceAndUnitAsync(resourceId, unitId);
            return balance?.Quantity >= requiredQuantity;
        }

        public async Task<IEnumerable<BalanceDto>> GetCurrentBalanceAsync()
        {
            var balances = await _balanceRepository.GetAllAsync();
            return balances.Select(b => new BalanceDto
            {
                ResourceId = b.ResourceId,
                ResourceName = b.Resource.Name,
                UnitId = b.UE_Id,
                UnitName = b.Ue.Name,
                Quantity = b.Quantity
            });
        }


        private async Task AdjustBalanceAsync(int resourceId, int unitId, int quantity, bool isIncrease)
        {
            var balance = await _balanceRepository.GetByResourceAndUnitAsync(resourceId, unitId);

            if (balance == null)
            {
                if (!isIncrease)
                    throw new InvalidOperationException("Cannot decrease non-existent balance");

                balance = new Balance
                {
                    ResourceId = resourceId,
                    UE_Id = unitId,
                    Quantity = quantity
                };
                await _balanceRepository.CreateAsync(balance);
            }
            else
            {
                balance.Quantity += isIncrease ? quantity : -quantity;

                if (balance.Quantity < 0)
                    throw new InvalidOperationException("Negative balance not allowed");

                await _balanceRepository.UpdateAsync(balance);
            }
        }

    }
}
