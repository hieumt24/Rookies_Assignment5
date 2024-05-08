using Assignment5.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment5.Domain.Entities
{
    public class Person : BaseEntity
    {
        [Required]
        [MaxLength(50, ErrorMessage = "First Name must have 50 characters or less")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Last Name must have 50 characters or less")]
        public string LastName { get; set; }
        public GenderType Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        public string BirthPlace { get; set; }
        public bool IsGraduated { get; set; }

        [NotMapped]
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }
}
