using EClaim.Domain.Common;
using EClaim.Domain.Entities;
using EClaim.Domain.Enums;

public class ClaimWorkflowStep : BaseEntity
{
    public required Status Action { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public string Comments { get; set; }

    public int ClaimRequestId { get; set; }
    public ClaimRequest ClaimRequest { get; set; }
}