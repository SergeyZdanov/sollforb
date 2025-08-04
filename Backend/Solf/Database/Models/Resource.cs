using Database.Enums;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public ICollection<Balance> Balances { get; set; }
        public ICollection<ResourceReceipt> ReceiptResources { get; set; }
        public ICollection<ResourceShipment> ShipmentResources { get; set; }
    }
}
