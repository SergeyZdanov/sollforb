namespace Frontend.Models
{
    public class ShipmentViewModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Status { get; set; }
        public List<ShipmentResourceViewModel> Resources { get; set; } = new();
    }
}