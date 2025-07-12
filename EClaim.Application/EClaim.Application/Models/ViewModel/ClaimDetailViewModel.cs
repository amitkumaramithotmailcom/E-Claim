namespace EClaim.Application.Models.ViewModel
{
    public class ClaimDetailViewModel
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<string> DocumentUrls { get; set; }
        public List<string> WorkflowHistory { get; set; }
    }
}
