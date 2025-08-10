using API.DTO.Balance;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/balance")]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBalance([FromQuery] BalanceFilterDto filter)
        {
            var balance = await _balanceService.GetCurrentBalanceAsync(filter.ResourceIds, filter.UeIds);
            return Ok(balance);
        }
    }
}