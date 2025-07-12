using EClaim.Application.Enum;

namespace EClaim.Application.Models
{
    public class UserCreateModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Role? Role { get; set; } = null;
        public bool? IsEmailVerified { get; set; } = null;
    }
}
