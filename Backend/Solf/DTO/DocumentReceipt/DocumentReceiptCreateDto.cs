namespace API.DTO.DocumentReceipt
{
    public class DocumentReceiptCreateDto
    {
        private DateTime? _date;

        public int Number { get; set; }

        public DateTime? Date
        {
            get => _date;
            set
            {
                if (value.HasValue)
                {
                    _date = value.Value.ToUniversalTime();
                }
                else
                {
                    _date = null;
                }
            }
        }
        public List<ReceiptResourceDto> Resources { get; set; } = new();
    }
}