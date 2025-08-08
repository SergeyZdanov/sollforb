using System.ComponentModel.DataAnnotations;

namespace API.DTO.Resourse
{
    public class ResourceCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
