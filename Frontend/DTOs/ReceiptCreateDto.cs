namespace Frontend.DTOs
{
    public class ReceiptCreateDto
    {
        public int Number { get; set; }
        public DateTime? Date { get; set; }
        public List<ReceiptResourceCreateDto> Resources { get; set; } = new();
    }

    public class ReceiptResourceCreateDto
    {
        public int ResourceId { get; set; }
        public int UnitId { get; set; }
        public int Quantity { get; set; }
    }
}