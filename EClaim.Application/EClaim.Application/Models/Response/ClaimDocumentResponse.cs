namespace EClaim.Application.Models.Response
{
    public class ClaimDocumentResponse 
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public int ClaimRequestId { get; set; }
        public ClaimRequestResponse ClaimRequest { get; set; }
       
        public DateTime CreatedAt { get; set; } 
    }
}
