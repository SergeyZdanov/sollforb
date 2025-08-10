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
        private readonly IDocumentShippingRepository _shippingRepository;

        public BalanceService(
            IBalanceRepository balanceRepository,
            IDocumentReceiptRepository receiptRepository,
            IDocumentShippingRepository shippingRepository)
        {
            _balanceRepository = balanceRepository;
            _receiptRepository = receiptRepository;
            _shippingRepository = shippingRepository;
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

        public async Task UpdateBalanceFromShippingAsync(int shippingDocumentId)
        {
            var document = await _shippingRepository.GetByIdAsync(shippingDocumentId, includeResources: true)
                ?? throw new KeyNotFoundException("Shipping document not found");

            foreach (var resource in document.ResourceShipments)
            {
                await AdjustBalanceAsync(
                    resource.ResourceId,
                    resource.UE_Id,
                    (int)resource.Quantity,
                    isIncrease: false);
            }
        }

        public async Task RevertBalanceFromShippingAsync(int shippingDocumentId)
        {
            var document = await _shippingRepository.GetByIdAsync(shippingDocumentId, includeResources: true)
                ?? throw new KeyNotFoundException("Shipping document not found");

            foreach (var resource in document.ResourceShipments)
            {
                await AdjustBalanceAsync(
                    resource.ResourceId,
                    resource.UE_Id,
                    (int)resource.Quantity,
                    isIncrease: true);
            }
        }

        public async Task<bool> HasSufficientQuantity(int resourceId, int unitId, decimal requiredQuantity)
        {
            var balance = await _balanceRepository.GetByResourceAndUnitAsync(resourceId, unitId);
            return balance?.Quantity >= requiredQuantity;
        }

        public async Task<IEnumerable<BalanceDto>> GetCurrentBalanceAsync(IEnumerable<int> resourceIds, IEnumerable<int> ueIds)
        {
            var balances = await _balanceRepository.GetFilteredAsync(resourceIds, ueIds);
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
                {
                    throw new InvalidOperationException("Negative balance not allowed");
                }

                if (balance.Quantity == 0)
                {
                    await _balanceRepository.DeleteAsync(balance.Id);
                }
                else
                {
                    await _balanceRepository.UpdateAsync(balance);
                }
            }
        }
    }
}