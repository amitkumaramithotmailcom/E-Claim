using System.ComponentModel.DataAnnotations;

namespace EClaim.Application.Models
{
    public class ClaimSubmissionModel
    {
        public int Id { get; set; }
        public int ClaimSubmissionId { get; set; }
        public int UserId { get; set; }
        [Required]
        public string ClaimType { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public List<ClaimDocumentModel> Documents { get; set; } = new();
    }
}
