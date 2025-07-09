using EClaim.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.Entities
{
    public class ClaimDocument : BaseEntity
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public int ClaimRequestId { get; set; }
        public ClaimRequest ClaimRequest { get; set; }
    }
}
