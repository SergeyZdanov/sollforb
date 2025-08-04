using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    // 2. Атрибут [Route] ОБЯЗАТЕЛЕН. Укажите маршрут.
    [Route("api/[controller]")]
    // 3. Класс должен быть PUBLIC и наследоваться от ControllerBase
    public class DiagnosticsController : ControllerBase
    {
        // 4. Метод (эндпоинт) должен быть PUBLIC
        // 5. У метода ОБЯЗАТЕЛЬНО должен быть HTTP-атрибут ([HttpGet], [HttpPost] и т.д.)
        [HttpGet]
        public IActionResult Check()
        {
            // 6. Метод должен возвращать один из типов результата (IActionResult, ActionResult<T> и т.д.)
            return Ok("Диагностика пройдена. Этот эндпоинт должен быть в Swagger!");
        }
    }
}
