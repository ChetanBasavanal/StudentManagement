using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DTO
{
    public class AddStudentRequestDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateOnly DOB { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string? Email { get; set; }
    }
}
