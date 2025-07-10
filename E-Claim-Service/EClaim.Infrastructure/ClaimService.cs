using EClaim.Domain.DTOs;
using EClaim.Domain.Entities;
using EClaim.Domain.Enums;
using EClaim.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Infrastructure
{
    public class ClaimService : IClaimService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;

        public ClaimService(AppDbContext context, IConfiguration config)
        {
            _dbContext = context;
            _config = config;
        }

        public async Task<ClaimRequest> GetClaimSubmission(int id)
        {

            var claim = await _dbContext.Claims
                .Include(a => a.Documents)
                .Include(a => a.WorkflowSteps).ThenInclude(u=>u.User)
                .Include(a => a.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            return claim;
        }

        public async Task<IEnumerable<ClaimRequest>> GetClaimDetails(ClaimSearchDto claimSearchDto)
        {
            var query = _dbContext.Claims
                  .Include(a => a.Documents)
                  .Include(a => a.WorkflowSteps).ThenInclude(a => a.User)
                  .Include(a => a.User)
                  .AsQueryable();

            if (Enum.IsDefined(typeof(Status), claimSearchDto.Status))
                query = query.Where(c => c.Status == claimSearchDto.Status);

            if (!string.IsNullOrEmpty(claimSearchDto.ClaimType))
                query = query.Where(c => c.ClaimType == claimSearchDto.ClaimType);

            if (claimSearchDto.FromDate.HasValue && claimSearchDto.ToDate.HasValue)
                query = query.Where(s => s.CreatedAt.Date >= claimSearchDto.FromDate.Value && s.CreatedAt.Date <= claimSearchDto.ToDate.Value);

            if (claimSearchDto.UserId > 0)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == claimSearchDto.UserId);

                if (user != null && user.Role == Role.Claimant)
                {
                    query = query.Where(s => s.UserId == claimSearchDto.UserId);
                }
            }

            return query;
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
                    UserId = claimSubmissionDto.UserId
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

        public async Task<ClaimRequest> UpdateStatus(ClaimStatusUpdateDto claimStatusUpdateDto)
        {

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var claim = await _dbContext.Claims
                            .FirstOrDefaultAsync(s => s.Id == claimStatusUpdateDto.Id);
                claim.Status = (Status)claimStatusUpdateDto.Action;

                _dbContext.Claims.Update(claim);
                await _dbContext.SaveChangesAsync();

                _dbContext.ClaimWorkflowSteps.Add(new ClaimWorkflowStep
                {
                    ClaimRequestId = claim.Id,
                    Comments = claimStatusUpdateDto.Comments,
                    Action = (Status)claimStatusUpdateDto.Action,
                    UserId = claimStatusUpdateDto.UserId
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
