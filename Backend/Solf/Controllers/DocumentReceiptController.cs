using API.DTO.DocumentReceipt;
using AutoMapper;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;
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
            var result = await _documentReceiptService.CreateAsync(_mapper.Map<DocumentReceipt>(documentReceiptCreateDto));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
                var document = await _documentReceiptService.GetByIdAsync(id);
                return Ok(document);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] DocumentReceiptCreateDto dto)
        {
            try
            {
                await _documentReceiptService.UpdateAsync(id, _mapper.Map<DocumentReceipt>(dto));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                await _documentReceiptService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredDocuments([FromQuery] DocumentReceiptFilterDto filter)
        {
            var documents = await _documentReceiptService.GetFilteredAsync(filter.StartDate, filter.EndDate, filter.DocumentNumbers, filter.ResourceIds, filter.Ue);
            return Ok(documents);
        }
    }
}

