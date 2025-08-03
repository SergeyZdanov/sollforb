using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Enums;

namespace Database.Models
{
    public class UE
    {
        [Key]
        int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public ICollection<Balance> Balances { get; set; }
        public ICollection<ResourceReceipt> ReceiptResources { get; set; }
        public ICollection<ResourceShipment> ShipmentResources { get; set; }
    }
}
