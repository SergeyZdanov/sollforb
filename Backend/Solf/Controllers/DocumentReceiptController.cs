using API.DTO.DocumentReceipt;
using AutoMapper;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentReceiptController : ControllerBase
    {
        private readonly IDocumentReceiptService _documentReceiptService;
        private readonly IMapper _mapper;
        public DocumentReceiptController(IDocumentReceiptService documentReceiptService, IMapper mapper)
        {
            _documentReceiptService = documentReceiptService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation("Создание документа на поступление")]
        public async Task<IActionResult> CreateClientAsync([FromBody] DocumentReceiptCreateDto documentReceiptCreateDto)
        {
            var documentEntity = await _documentReceiptService.CreateAsync(_mapper.Map<DocumentReceipt>(documentReceiptCreateDto));
            var result = await _documentReceiptService.GetByIdAsync(documentEntity.Id);
            return Ok(_mapper.Map<DocumentReceiptDto>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            var document = await _documentReceiptService.GetByIdAsync(id);
            if (document == null)
                return NotFound();

            return Ok(_mapper.Map<DocumentReceiptDto>(document));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] DocumentReceiptCreateDto dto)
        {
            await _documentReceiptService.UpdateAsync(id, _mapper.Map<DocumentReceipt>(dto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            await _documentReceiptService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredDocuments([FromQuery] DocumentReceiptFilterDto filter)
        {
            var documents = await _documentReceiptService.GetFilteredAsync(filter.StartDate, filter.EndDate, filter.DocumentNumbers, filter.ResourceIds, filter.Ue);
            var resultDto = _mapper.Map<IEnumerable<DocumentReceiptDto>>(documents);
            return Ok(resultDto);
        }
    }
}