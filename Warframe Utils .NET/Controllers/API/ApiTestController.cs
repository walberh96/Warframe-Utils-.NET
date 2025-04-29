using Microsoft.AspNetCore.Mvc;

namespace Warframe_Utils_.NET.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiTestController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var response = new
            {
                message = "Api Test"
            };

            return Ok(response);
        }
    }
}
