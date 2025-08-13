namespace API.DTO.DocumentReceipt
{
    public class DocumentReceiptDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Date { get; set; }
        public List<ReceiptResourceResponseDto> Resources { get; set; }
    }
}