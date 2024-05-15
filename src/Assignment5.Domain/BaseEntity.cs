using System.ComponentModel.DataAnnotations;

namespace Assignment5.Domain
{
    public class BaseEntity
    {
        
        [Required]
        [Key]
        public Guid Id { get; set; }
    }
}
