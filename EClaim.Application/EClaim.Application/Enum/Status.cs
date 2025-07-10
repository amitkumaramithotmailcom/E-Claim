using System.ComponentModel.DataAnnotations;

namespace EClaim.Application.Enum
{
    public enum Status
    {
        [Display(Name = "Claim Submitted")]
        Submitted = 1,

        [Display(Name = "Claim Reviewed")]
        Reviewed = 2,

        [Display(Name = "Claim Approved")]
        Approved = 3,

        [Display(Name = "Claim Rejected")]
        Rejected = 4
    }
}
