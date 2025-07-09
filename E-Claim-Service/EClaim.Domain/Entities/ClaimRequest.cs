using EClaim.Domain.Common;
using EClaim.Domain.Enums;

namespace EClaim.Domain.Entities
{
    public class ClaimRequest : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string ClaimType { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; } = Status.Submitted;
        public ICollection<ClaimDocument> Documents { get; set; }
        public ICollection<ClaimWorkflowStep> WorkflowSteps { get; set; }
    }
}
