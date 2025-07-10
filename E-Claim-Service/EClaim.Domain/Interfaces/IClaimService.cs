using EClaim.Domain.DTOs;
using EClaim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.Interfaces
{
    public interface IClaimService
    {
        Task<ClaimRequest> ClaimSubmission(ClaimSubmissionDto claimSubmissionDto);
        Task<ClaimRequest> GetClaimSubmission(int id);
        Task<IEnumerable<ClaimRequest>> GetClaimDetails(ClaimSearchDto claimSearchDto);
        Task<ClaimRequest> UpdateStatus(ClaimStatusUpdateDto claimStatusUpdateDto);
    }
}
