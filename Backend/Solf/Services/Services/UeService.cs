using Database.Enums;
using Database.Interfaces;
using Database.Models;
using Services.Interfaces;

namespace Services.Services
{
    public class UeService : IUeService
    {
        private readonly IUeRepository _ueRepository;

        public UeService(IUeRepository ueRepository)
        {
            _ueRepository = ueRepository;
        }

        public async Task<UE> CreateAsync(UE uE)
        {
            if (await _ueRepository.ExistsByNameAsync(uE.Name))
                throw new Exception("Not unique name");

            return await _ueRepository.CreateAsync(uE);
        }

        public async Task<UE> GetAByIdAsync(int id)
        {
            return await _ueRepository.GetByIdAsync(id);
        }

        public async Task<List<UE>> GetAllAsync(bool isActive)
        {
            if (isActive)
                return await _ueRepository.GetAllActiveAsync();
            else
                return await _ueRepository.GetAllAsync();
        }

        public async Task UpdateAsync(int id, UE uE)
        {
            uE.Id = id;
            await _ueRepository.UpdateAsync(uE);
        }

        public async Task DeleteAsync(int id)
        {
            if (await _ueRepository.HasDependenciesAsync(id))
            {
                var result = await _ueRepository.GetByIdAsync(id);
                result.Status = EntityStatus.Archived;
                await UpdateAsync(id, result);
            }
            else
            {
                await _ueRepository.DeleteAsync(id);
            }
        }

        public async Task<UE?> GetActiveByIdAsync(int id)
        {
            return await _ueRepository.GetActiveByIdAsync(id);
        }

        public async Task ArchiveClientAsync(int id)
        {
            var result = await _ueRepository.GetByIdAsync(id);
            result.Status = EntityStatus.Archived;
            await _ueRepository.UpdateAsync(result);
        }
    }
}
