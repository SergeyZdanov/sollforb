using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class DocumentReceipt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public ICollection<ResourceReceipt> ResourceReceipts { get; set; }
    }
}
