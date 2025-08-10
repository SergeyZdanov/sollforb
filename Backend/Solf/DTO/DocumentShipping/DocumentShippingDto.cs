namespace API.DTO.DocumentShipping
{
    public class DocumentShippingDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ClientId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public List<ShippingResourceResponseDto> Resources { get; set; }
    }
}
