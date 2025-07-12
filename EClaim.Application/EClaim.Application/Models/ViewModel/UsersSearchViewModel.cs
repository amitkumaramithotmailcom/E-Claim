using EClaim.Application.Enum;
using EClaim.Application.Models.Response;

namespace EClaim.Application.Models.ViewModel
{
    public class UsersSearchViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public string? IsEmailVerified { get; set; } = null;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }


        public List<string> Roles { get; set; } = Utility.Roles;
        public List<string> UserVerified { get; set; } = Utility.UserVerified;
        

        public IEnumerable<UsersViewModel>? Results { get; set; }
    }
}
