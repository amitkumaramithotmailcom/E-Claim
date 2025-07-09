using EClaim.Domain.DTOs;
using EClaim.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Claim_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimSubmissionController : ControllerBase
    {
        private readonly IClaimSubmissionService _claimSubmissionService;

        public ClaimSubmissionController(IClaimSubmissionService claimSubmissionService)
        {
            _claimSubmissionService = claimSubmissionService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClaimSubmissionDto claimSubmissionDto)
        {
            var result = await _claimSubmissionService.ClaimSubmission(claimSubmissionDto);
            return Ok(result);
        }
    }
}
