using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Enums;

namespace Database.Models
{
    public class DocumentShipping
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public int ClientId{ get; set; }

        public DateTime Date { get; set; }

        public  DocumentStatus Status { get; set; } = DocumentStatus.Draft;


        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }

        public ICollection<ResourceShipment> ResourceShipments { get; set; }
    }
}
