using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Route("")]

        public async Task<IActionResult> GetAllClientAsync()
        {
            var res = await _clientService.GetAllAsync();
            return Ok(res);
        }
    }
}
