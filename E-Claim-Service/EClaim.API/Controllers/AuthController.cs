using EClaim.Domain.DTOs;
using EClaim.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Claim_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            _logger.LogInformation("User registration request", dto);
            var result = await _authService.RegisterAsync(dto);
            _logger.LogInformation("User Register success", result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            _logger.LogInformation("User login request", dto);
            var result = await _authService.LoginAsync(dto);
            _logger.LogInformation("User login success", result);
            return Ok(result);
        }

        [HttpGet("ConfirmEmail/{email}/{token}")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var result = await _authService.ConfirmEmail(email, token);
            return Ok(true);
        }
    }
}
