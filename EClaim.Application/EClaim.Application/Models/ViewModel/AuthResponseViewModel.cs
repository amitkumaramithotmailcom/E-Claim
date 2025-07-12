namespace EClaim.Application.Models.ViewModel
{
    public class AuthResponseViewModel
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
