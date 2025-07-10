using EClaim.Application.Models.Response;

namespace EClaim.Application.Models.ViewModel
{
    public class ClaimSearchViewModel
    {
        public int UserId { get; set; }
        public string? Status { get; set; }
        public string? ClaimType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<string> StatusOptions { get; set; } = Utility.StatusOptions;
        public List<string> ClaimTypeOptions { get; set; } = Utility.ClaimTypeOptions;

        public IEnumerable<ClaimRequestResponse>? Results { get; set; }
    }
}
