using EClaim.Domain.DTOs;
using EClaim.Domain.Entities;
using EClaim.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Claim_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;
        private readonly ILogger<AuthController> _logger;

        public ClaimController(IClaimService claimService, ILogger<AuthController> logger)
        {
            _claimService = claimService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"User claim request search for {id}");
            var result = await _claimService.GetClaimSubmission(id);
            _logger.LogInformation("User claim request search response", result);
            return Ok(result);
        }

        [HttpGet("GetClaimDetails")]
        public async Task<IActionResult> Get([FromQuery] ClaimSearchDto claimSearchDto)
        {
            _logger.LogInformation($"User claim request search", claimSearchDto);
            var result = await _claimService.GetClaimDetails(claimSearchDto);
            _logger.LogInformation("User claim request search response", result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClaimSubmissionDto claimSubmissionDto)
        {
            _logger.LogInformation("User claim request save request", claimSubmissionDto);
            var result = await _claimService.ClaimSubmission(claimSubmissionDto);
            _logger.LogInformation("User claim request save response", result);
            return Ok(result);
        }

        [HttpPatch("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(ClaimStatusUpdateDto claimStatusUpdateDto)
        {
            _logger.LogInformation("User claim request update request", claimStatusUpdateDto);
            var result = await _claimService.UpdateStatus(claimStatusUpdateDto);
            _logger.LogInformation("User claim request update request", result);
            return Ok(result);
        }
    }
}
