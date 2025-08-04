using Database.Enums;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{

    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Adress { get; set; }

        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public ICollection<DocumentShipping> DocumentShippings { get; set; }
    }
}
