using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagement.Model
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name should not be Empty")]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "First Name should not be Empty")]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth should not be Empty")]
        [DataType(DataType.Text)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EnumDataType(typeof(Gender), ErrorMessage = "Please provide Male or Female")]
        public Gender Gender { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact Number must be 10 digits")]
        [Column("PhoneNumber")]
        public string ContactNumber { get; set; }

        [Required]
        public float Weight { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Height { get; set; }

        [Required, StringLength(50)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Your Email is not valid.")]
        public string Email { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, StringLength(500)]
        public string MedicalComments { get; set; } 

        [Required(ErrorMessage = "Please enter true or false")]
        public bool AnyMedicationsTaking { get; set; }

        [DataType(DataType.Text)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Text)]
        public DateTime UpdatedDate { get; set; }
    }

    public enum Gender 
    {
        Male,
        Female
    }
}
