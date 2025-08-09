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
        public async Task<IActionResult> GetBalance()
        {
                var balance = await _balanceService.GetCurrentBalanceAsync();

                return Ok(balance);
        }
    }
}
