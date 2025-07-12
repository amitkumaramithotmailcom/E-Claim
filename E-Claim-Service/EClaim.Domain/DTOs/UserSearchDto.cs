using EClaim.Domain.Common;
using EClaim.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.DTOs
{
    public class UserSearchDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Role? Role { get; set; } = null;
        public bool? IsEmailVerified { get; set; } = null;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
