using EClaim.Domain.Common;
using EClaim.Domain.Enums;

namespace EClaim.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public bool IsEmailVerified { get; set; } = false;
    }
}
