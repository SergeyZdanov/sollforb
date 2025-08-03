using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Enums;

namespace Database.Models
{
    public class Client
    {
        [Key]
        int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Adress { get; set; }

        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public ICollection<DocumentShipping> DocumentShippings { get; set; }
    }
}
