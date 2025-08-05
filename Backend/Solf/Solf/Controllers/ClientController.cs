using API.DTO.Client;
using AutoMapper;
using Database.Enums;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation("Создание клиента")]
        public async Task<IActionResult> CreateClientAsync([FromBody] ClientCreateDto client)
        {
            var result = await _clientService.CreateAsync(_mapper.Map<Client>(client));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Получение клиента")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _clientService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        [SwaggerOperation("Получение всех клиентов")]
        public async Task<IActionResult> GetAllClientAsync()
        {
            var result = await _clientService.GetAllAsync();
            return Ok(result);
        }

        [HttpPut]
        [Route("")]
        [SwaggerOperation("Update клиента")]
        public async Task<IActionResult> UpdateAsync(int id, ClientUpdateDto clientUpdateDto)
        {
            await _clientService.UpdateAsync(id, _mapper.Map<Client>(clientUpdateDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Удаление клиента")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _clientService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Архивация клиента")]
        public async Task<IActionResult> ArchiveClientAsync(int id)
        {
            await _clientService.ArchiveClientAsync(id);
            return NoContent();
        }
    }
}
