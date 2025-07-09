using EClaim.Domain.DTOs;
using EClaim.Domain.Entities;
using EClaim.Domain.Enums;
using EClaim.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Infrastructure
{
    public class ClaimSubmissionService : IClaimSubmissionService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;

        public ClaimSubmissionService(AppDbContext context, IConfiguration config)
        {
            _dbContext = context;
            _config = config;
        }

        public async Task<ClaimRequest> ClaimSubmission(ClaimSubmissionDto claimSubmissionDto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var claim = new ClaimRequest
                {
                    ClaimType = claimSubmissionDto.ClaimType,
                    Description = claimSubmissionDto.Description,
                    UserId = claimSubmissionDto.UserId,
                    Status = Status.Submitted
                };

                _dbContext.Claims.Add(claim);
                await _dbContext.SaveChangesAsync();

                if (claimSubmissionDto.Documents != null)
                {
                    foreach (var file in claimSubmissionDto.Documents)
                    {
                        _dbContext.ClaimDocuments.Add(new ClaimDocument
                        {
                            FileName = file.FileName,
                            FilePath = file.FilePath,
                            ClaimRequestId = claim.Id
                        });
                    }
                }
                await _dbContext.SaveChangesAsync();

                _dbContext.ClaimWorkflowSteps.Add(new ClaimWorkflowStep
                {
                    ClaimRequestId = claim.Id,
                    Comments = "Claim Submitted",
                    Action = Status.Submitted,
                    PerformedBy = claimSubmissionDto.UserId
                });

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return claim;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
             
                throw new ApplicationException("Claim submission failed.", ex);
            }
        }

    }
}
