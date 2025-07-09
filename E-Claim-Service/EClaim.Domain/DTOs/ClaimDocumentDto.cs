using EClaim.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.DTOs
{
    public class ClaimDocumentDto
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public int ClaimRequestId { get; set; }
    }
}
