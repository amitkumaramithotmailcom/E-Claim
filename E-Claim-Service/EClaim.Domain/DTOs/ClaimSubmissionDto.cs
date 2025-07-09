using EClaim.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Domain.DTOs
{
    public class ClaimSubmissionDto
    {
        public int ClaimSubmissionId { get; set; }
        public int UserId { get; set; }

        [Required]
        public string ClaimType { get; set; }

        [Required]
        public string Description { get; set; }

        public List<ClaimDocumentDto> Documents { get; set; } = new();
    }
}
