namespace Frontend.Models
{
    public class ReceiptViewModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public List<ReceiptResourceViewModel> Resources { get; set; } = new();
    }
}
