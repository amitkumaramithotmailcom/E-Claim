using EClaim.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.DTOs
{
    public class ClaimStatusUpdateDto
    {
        public int Id { get; set; }
        public Status Action { get; set; }
        public int UserId { get; set; }
        public string Comments { get; set; }
    }
}
