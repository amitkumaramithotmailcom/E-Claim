using EClaim.Application.Enum;
using EClaim.Application.Models.Response;

namespace EClaim.Application.Models.ViewModel
{
    public class ClaimStatusUpdateViewModel
    {
        public int Id { get; set; }
        public Status Action { get; set; }
        public int UserId { get; set; }
        public string Comments { get; set; }
    }
}
