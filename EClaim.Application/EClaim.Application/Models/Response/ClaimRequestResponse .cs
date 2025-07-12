using EClaim.Application.Enum;

namespace EClaim.Application.Models.Response
{
    public class ClaimRequestResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserResponse User { get; set; }
        public string ClaimType { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ClaimDocumentResponse> Documents { get; set; }
        public ICollection<ClaimWorkflowStepResponse> WorkflowSteps { get; set; }
    }
}
