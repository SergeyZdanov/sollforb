using Database.Enums;
using Database.Interfaces;
using Database.Models;
using Services.Interfaces;

namespace Services.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> CreateAsync(Client client)
        {
            return await _clientRepository.CreateAsync(client);
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _clientRepository.GetByIdAsync(id);
        }
        public async Task<List<Client>> GetAllAsync()
        {
            return await _clientRepository.GetAllAsync();
        }

        public async Task UpdateAsync(int id, Client client)
        {
            client.Id = id;
            await _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteAsync(int id)
        {
            if (await _clientRepository.HasDependenciesAsync(id))
            {
                await ArchiveClientAsync(id);
            }
            else
            {
                await _clientRepository.DeleteAsync(id);
            }
        }

        public async Task ArchiveClientAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if (result != null)
            {
                result.Status = EntityStatus.Archived;
                await _clientRepository.UpdateAsync(result);
            }
        }
    }
}