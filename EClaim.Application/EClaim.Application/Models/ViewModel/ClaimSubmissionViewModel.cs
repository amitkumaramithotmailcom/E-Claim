using System.ComponentModel.DataAnnotations;

namespace EClaim.Application.Models.Claim
{
    public class ClaimSubmissionViewModel
    {
        public int UserId { get; set; }
        [Required]
        public string ClaimType { get; set; }

        [Required]
        public string Description { get; set; }

        public List<IFormFile> Documents { get; set; } = new();
    }
}
