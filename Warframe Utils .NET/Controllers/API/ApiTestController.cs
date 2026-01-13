using Microsoft.AspNetCore.Mvc;

namespace Warframe_Utils_.NET.Controllers.API
{
    /// <summary>
    /// ApiTestController provides a simple API endpoint for testing purposes.
    /// This is useful for debugging and verifying that the API infrastructure is working.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ApiTestController : Controller
    {
        /// <summary>
        /// Simple GET endpoint that returns a test message.
        /// 
        /// Example Response:
        /// {
        ///   "message": "Api Test"
        /// }
        /// 
        /// Useful for:
        /// - Checking if the API is responding
        /// - Load testing
        /// - Network connectivity verification
        /// </summary>
        /// <returns>IActionResult - JSON object with test message</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var response = new
            {
                message = "Api Test",
                timestamp = DateTime.UtcNow,
                status = "running"
            };

            return Ok(response);
        }
    }
}
