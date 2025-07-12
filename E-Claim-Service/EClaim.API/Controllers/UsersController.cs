using EClaim.Domain.DTOs;
using EClaim.Domain.Entities;
using EClaim.Domain.Interfaces;
using EClaim.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Claim_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public UsersController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"User search for {id}");
            var result = await _userService.GetUser(id);
            _logger.LogInformation("User search response", result);
            return Ok(result);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> Get([FromQuery] UserSearchDto userSearchDto)
        {
            _logger.LogInformation($"User search request", userSearchDto);
            var result = await _userService.GetAllUser(userSearchDto);
            _logger.LogInformation($"User search response", result);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserSearchDto userSearchDto)
        {
            _logger.LogInformation($"User update request", userSearchDto);
            var result = await _userService.EditAsync(userSearchDto);
            _logger.LogInformation($"User update response", result);
            return Ok(result);
        }
    }
}
