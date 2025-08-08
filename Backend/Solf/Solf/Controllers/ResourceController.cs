using API.DTO.Resourse;
using AutoMapper;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;
        private readonly IMapper _mapper;

        public ResourceController(IResourceService resourceService, IMapper mapper)
        {
            _resourceService = resourceService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation("Создание ресурса")]
        public async Task<IActionResult> CreateClientAsync([FromBody] ResourceCreateDto resource)
        {
            var result = await _resourceService.CreateAsync(_mapper.Map<Resource>(resource));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Получение ресурса")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _resourceService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [SwaggerOperation("Получение всех ресурсов")]
        public async Task<IActionResult> GetAllClientAsync([FromQuery] bool isActive)
        {
            var result = await _resourceService.GetAllAsync(isActive);
            return Ok(result);
        }

        [HttpPut]
        [Route("")]
        [SwaggerOperation("Обновление ресурса")]
        public async Task<IActionResult> UpdateAsync(int id, ResourceCreateDto clientUpdateDto)
        {
            await _resourceService.UpdateAsync(id, _mapper.Map<Resource>(clientUpdateDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Удаление ресурса")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _resourceService.DeleteAsync(id);
            return NoContent();
        }
    }
}
