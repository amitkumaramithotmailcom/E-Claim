using EClaim.Domain.Common;
using EClaim.Domain.Entities;
using EClaim.Domain.Enums;

public class ClaimWorkflowStep : BaseEntity
{
    public required Status Action { get; set; }
    public int PerformedBy { get; set; }
    public string Comments { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    public int ClaimRequestId { get; set; }
    public ClaimRequest ClaimRequest { get; set; }
}