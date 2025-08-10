namespace API.DTO.DocumentShipping
{
    public class ShippingResourceResponseDto
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal Quantity { get; set; }
    }
}
