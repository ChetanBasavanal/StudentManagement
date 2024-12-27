using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DTO
{
    public class AddDepartmentDTO
    {
        [Required]
        public string? DepartmentName { get; set; }
        [Required]
        public string? Head { get; set; }
    }
}
