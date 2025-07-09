using EClaim.Domain.Common;
using EClaim.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public Role Role { get; set; }
        public bool IsEmailVerified { get; set; } = false;
    }
}
