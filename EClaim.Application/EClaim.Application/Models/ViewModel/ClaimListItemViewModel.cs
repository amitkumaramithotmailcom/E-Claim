namespace EClaim.Application.Models.ViewModel
{
    public class ClaimListItemViewModel
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
