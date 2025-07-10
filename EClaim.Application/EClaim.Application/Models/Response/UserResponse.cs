using EClaim.Application.Enum;

namespace EClaim.Application.Models.Response
{
    public class UserResponse
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public Role Role { get; set; }
        public bool IsEmailVerified { get; set; } = false;
    }
}
