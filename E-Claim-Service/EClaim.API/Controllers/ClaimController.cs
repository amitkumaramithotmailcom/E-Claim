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

        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _claimService.GetClaimSubmission(id);
            return Ok(result);
        }

        [HttpGet("GetClaimDetails")]
        public async Task<IActionResult> Get([FromQuery] ClaimSearchDto claimSearchDto)
        {
            var result = await _claimService.GetClaimDetails(claimSearchDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClaimSubmissionDto claimSubmissionDto)
        {
            var result = await _claimService.ClaimSubmission(claimSubmissionDto);
            return Ok(result);
        }

        [HttpPatch("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(ClaimStatusUpdateDto claimStatusUpdateDto)
        {
            var result = await _claimService.UpdateStatus(claimStatusUpdateDto);
            return Ok(result);
        }
    }
}
