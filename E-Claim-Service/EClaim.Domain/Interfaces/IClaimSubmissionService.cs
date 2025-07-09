using EClaim.Domain.DTOs;
using EClaim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.Interfaces
{
    public interface IClaimSubmissionService
    {
        Task<ClaimRequest> ClaimSubmission(ClaimSubmissionDto claimSubmissionDto);
    }
}
