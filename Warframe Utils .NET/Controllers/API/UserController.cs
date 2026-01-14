using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Warframe_Utils_.NET.Controllers.API
{
    /// <summary>
    /// UserController handles user information retrieval
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(
            UserManager<IdentityUser> userManager,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Get current authenticated user information
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<object>> GetCurrentUser()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                email = user.Email,
                userName = user.UserName,
                emailConfirmed = user.EmailConfirmed,
                isAuthenticated = true
            });
        }

        /// <summary>
        /// Check if user is authenticated (public endpoint)
        /// </summary>
        [HttpGet("check")]
        public IActionResult CheckAuth()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Ok(new { isAuthenticated = false });

            return Ok(new { isAuthenticated = true });
        }
    }
}
