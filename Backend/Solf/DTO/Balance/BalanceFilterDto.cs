namespace API.DTO.Balance
{
    public class BalanceFilterDto
    {
        public List<int> ResourceIds { get; set; } = new();
        public List<int> UeIds { get; set; } = new();
    }
}
