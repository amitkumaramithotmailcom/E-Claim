using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.Entities
{
    public class AppLog
    {
        public int Id { get; set; }
        public string? Level { get; set; }
        public string? Message { get; set; }
        public string? Category { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
