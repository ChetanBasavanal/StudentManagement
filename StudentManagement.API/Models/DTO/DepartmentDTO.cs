using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DTO
{
    public class DepartmentDTO
    {
        public int DepartmentID { get; set; }

        [Required]
        public string DepartmentName { get; set; }
        [Required]
        public string Head { get; set; }
    }
}
