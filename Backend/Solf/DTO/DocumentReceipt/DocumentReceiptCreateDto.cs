namespace API.DTO.DocumentReceipt
{
    public class DocumentReceiptCreateDto
    {
        public int Number { get; set; }
        public DateTime? Date { get; set; }
        public List<ReceiptResourceDto> Resources { get; set; } = new();
    }
}
