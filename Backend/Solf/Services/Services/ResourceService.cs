using Database.Enums;
using Database.Interfaces;
using Database.Models;
using Services.Interfaces;


namespace Services.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<Resource> CreateAsync(Resource resource)
        {
            if (await _resourceRepository.ExistsByNameAsync(resource.Name))
                throw new Exception("Not unique name");

            return await _resourceRepository.CreateAsync(resource);
        }

        public async Task<Resource> GetByIdAsync(int id)
        {
            return await _resourceRepository.GetByIdAsync(id);
        }

        public async Task<List<Resource>> GetAllAsync(bool isActive)
        {
            if (isActive)
                return await _resourceRepository.GetAllActiveAsync();
            else
                return await _resourceRepository.GetAllAsync();
        }

        public async Task UpdateAsync(int id, Resource resource)
        {
            resource.Id = id;
            await _resourceRepository.UpdateAsync(resource);
        }

        public async Task DeleteAsync(int id)
        {
            if (await _resourceRepository.HasDependenciesAsync(id))
            {
                var result = await _resourceRepository.GetByIdAsync(id);
                result.Status = EntityStatus.Archived;
                await UpdateAsync(id, result);
            }
            else
            {
                await _resourceRepository.DeleteAsync(id);
            }
        }

        public async Task<Resource?> GetActiveByIdAsync(int id)
        {
            return await _resourceRepository.GetActiveByIdAsync(id);
        }

        public async Task ArchiveClientAsync(int id)
        {
            var result = await _resourceRepository.GetByIdAsync(id);
            result.Status = EntityStatus.Archived;
            await _resourceRepository.UpdateAsync(result);
        }
    }
}
