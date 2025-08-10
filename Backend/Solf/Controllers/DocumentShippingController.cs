using API.DTO.DocumentShipping;
using AutoMapper;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentShippingController : ControllerBase
    {
        private readonly IDocumentShippingService _documentShippingService;
        private readonly IMapper _mapper;

        public DocumentShippingController(IDocumentShippingService documentShippingService, IMapper mapper)
        {
            _documentShippingService = documentShippingService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation("Создание документа на отгрузку")]
        public async Task<IActionResult> CreateDocumentAsync([FromBody] DocumentShippingCreateDto dto)
        {
            var documentEntity = await _documentShippingService.CreateAsync(_mapper.Map<DocumentShipping>(dto));
            var result = await _documentShippingService.GetByIdAsync(documentEntity.Id, includeRelated: true);
            return Ok(_mapper.Map<DocumentShippingDto>(result));
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Получение документа отгрузки по ID")]
        public async Task<IActionResult> GetDocumentByIdAsync(int id)
        {
            var document = await _documentShippingService.GetByIdAsync(id, includeRelated: true);
            if (document == null)
                return NotFound();

            return Ok(_mapper.Map<DocumentShippingDto>(document));
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Обновление документа отгрузки")]
        public async Task<IActionResult> UpdateDocumentAsync(int id, [FromBody] DocumentShippingCreateDto dto)
        {
            await _documentShippingService.UpdateAsync(id, _mapper.Map<DocumentShipping>(dto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Удаление документа отгрузки")]
        public async Task<IActionResult> DeleteDocumentAsync(int id)
        {
            await _documentShippingService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation("Фильтрация документов отгрузки")]
        public async Task<IActionResult> GetFilteredDocumentsAsync([FromQuery] DocumentShippingFilterDto filter)
        {
            var documents = await _documentShippingService.GetFilteredAsync(
                filter.StartDate,
                filter.EndDate,
                filter.DocumentNumbers,
                filter.ResourceIds,
                filter.UeIds,
                filter.ClientIds);

            var resultDto = _mapper.Map<IEnumerable<DocumentShippingDto>>(documents);
            return Ok(resultDto);
        }

        [HttpPost("{id}/sign")]
        [SwaggerOperation("Подписать документ отгрузки")]
        public async Task<IActionResult> SignDocumentAsync(int id)
        {
            await _documentShippingService.SignAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/revert-sign")]
        [SwaggerOperation("Отозвать подписание документа отгрузки")]
        public async Task<IActionResult> RevertSignDocumentAsync(int id)
        {
            await _documentShippingService.RevertSignAsync(id);
            return NoContent();
        }
    }
}