using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class ResourceShipment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ResourceId { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [Required]
        public int UE_Id { get; set; }

        [Required]
        public int Quantity { get; set; }


        [ForeignKey(nameof(DocumentId))]
        public DocumentShipping DocumentShipping { get; set; }

        [ForeignKey(nameof(ResourceId))]
        public Resource Resource { get; set; }

        [ForeignKey(nameof(UE_Id))]
        public UE Ue { get; set; }
    }
}
