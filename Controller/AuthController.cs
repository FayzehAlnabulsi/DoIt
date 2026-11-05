using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoIt
{
    [Route("api/[controller]")]
    [ApiController]
   public class ValueController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello from ValueController");
        }
    }
}
