namespace API.DTO.DocumentReceipt
{
    public class DocumentReceiptFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> DocumentNumbers { get; set; } = new();
        public List<int> ResourceIds { get; set; } = new();
        public List<int> Ue { get; set; } = new();
    }
}
