using System.ComponentModel.DataAnnotations;

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
