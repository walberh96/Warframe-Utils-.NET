using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Warframe_Utils_.NET.Controllers.API
{
    /// <summary>
    /// AuthController handles API-based authentication (login, register, logout)
    /// This is separate from the Razor Pages Identity UI and doesn't require anti-forgery tokens
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<AuthController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(
                request.Email, 
                request.Password, 
                request.RememberMe, 
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in: {Email}", request.Email);
                var user = await _userManager.FindByEmailAsync(request.Email);
                return Ok(new { 
                    success = true, 
                    email = user?.Email,
                    message = "Login successful" 
                });
            }

            if (result.RequiresTwoFactor)
            {
                return BadRequest(new { success = false, message = "Two-factor authentication required" });
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out: {Email}", request.Email);
                return BadRequest(new { success = false, message = "Account locked out" });
            }

            _logger.LogWarning("Failed login attempt for: {Email}", request.Email);
            return BadRequest(new { success = false, message = "Invalid email or password" });
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new { success = false, message = "Passwords do not match" });
            }

            var user = new IdentityUser 
            { 
                UserName = request.Email, 
                Email = request.Email,
                EmailConfirmed = true // Auto-confirm for development
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account: {Email}", request.Email);
                
                // Automatically sign in the user after registration
                await _signInManager.SignInAsync(user, isPersistent: false);
                
                return Ok(new { 
                    success = true, 
                    email = user.Email,
                    message = "Registration successful" 
                });
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { success = false, message = "Registration failed", errors });
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");
            return Ok(new { success = true, message = "Logout successful" });
        }

        /// <summary>
        /// Check authentication status
        /// </summary>
        [HttpGet("status")]
        public async Task<ActionResult> GetStatus()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId!);
                return Ok(new { 
                    isAuthenticated = true, 
                    email = user?.Email 
                });
            }

            return Ok(new { isAuthenticated = false });
        }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }

    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
