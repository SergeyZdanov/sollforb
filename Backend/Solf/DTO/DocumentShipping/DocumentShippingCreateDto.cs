namespace API.DTO.DocumentShipping
{
    public class DocumentShippingCreateDto
    {
        public int Number { get; set; }
        public int ClientId { get; set; }
        public DateTime? Date { get; set; }
        public List<ShippingResourceDto> Resources { get; set; } = new();
    }
}
