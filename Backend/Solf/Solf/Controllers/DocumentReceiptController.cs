using API.DTO.Client;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentReceiptController : ControllerBase
    {
        private readonly DocumentReceiptService _documentReceiptService;
        private readonly IMapper _mapper;
        public DocumentReceiptController(DocumentReceiptService documentReceiptService, IMapper mapper)
        {
            _documentReceiptService = documentReceiptService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation("Создание документа на поступление")]
        public async Task<IActionResult> CreateClientAsync([FromBody] ClientCreateDto client)
        {
            //var result = await _documentReceiptService.CreateAsync(_mapper.Map<Client>(client));
            return Ok();
        }
    }
}
