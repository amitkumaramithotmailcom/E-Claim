﻿using EClaim.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.DTOs
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; } = Role.Claimant;
    }
}
