using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.Enums
{
    public enum Status
    {
        Submitted = 1,
        Reviewed = 2,
        Approved = 3,
        Rejected = 4
    }

}
