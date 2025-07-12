using EClaim.Application.Enum;

namespace EClaim.Application.Models.ViewModel
{
    public class UsersViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Role Role { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
