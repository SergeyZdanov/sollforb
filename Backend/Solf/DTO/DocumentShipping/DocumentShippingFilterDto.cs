namespace API.DTO.DocumentShipping
{
    public class DocumentShippingFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> DocumentNumbers { get; set; } = new();
        public List<int> ResourceIds { get; set; } = new();
        public List<int> UeIds { get; set; } = new();
        public List<int> ClientIds { get; set; } = new();
    }
}
