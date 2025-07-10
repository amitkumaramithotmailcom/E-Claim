using EClaim.Application.Enum;

namespace EClaim.Application.Models.Response
{
    public class ClaimWorkflowStepResponse
    {
        public Status Action { get; set; }
        public int UserId { get; set; }
        public UserResponse User { get; set; }

        public string Comments { get; set; }
        public DateTime CreatedAt { get; set; } 

        public int ClaimRequestId { get; set; }
        public ClaimRequestResponse ClaimRequest { get; set; }
    }
}