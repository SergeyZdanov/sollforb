using API.DTO.UE;
using AutoMapper;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UeController : ControllerBase
    {
        private readonly IUeService _ueService;
        private readonly IMapper _mapper;

        public UeController(IUeService ueService, IMapper mapper)
        {
            _ueService = ueService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation("Создание ресурса")]
        public async Task<IActionResult> CreateClientAsync([FromBody] UeCreateDto resource)
        {
            var result = await _ueService.CreateAsync(_mapper.Map<UE>(resource));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Получение ресурса")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _ueService.GetAByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [SwaggerOperation("Получение всех ресурсов")]
        public async Task<IActionResult> GetAllClientAsync([FromQuery] bool isActive)
        {
            var result = await _ueService.GetAllAsync(isActive);
            return Ok(result);
        }

        [HttpPut]
        [Route("")]
        [SwaggerOperation("Обновление ресурса")]
        public async Task<IActionResult> UpdateAsync(int id, UeCreateDto clientUpdateDto)
        {
            await _ueService.UpdateAsync(id, _mapper.Map<UE>(clientUpdateDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Удаление ресурса")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _ueService.DeleteAsync(id);
            return NoContent();
        }

    }
}
