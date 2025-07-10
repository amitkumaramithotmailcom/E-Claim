using EClaim.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.DTOs
{
    public class ClaimSearchDto
    {
        public int UserId { get; set; }
        public Status Status { get; set; }
        public string? ClaimType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
