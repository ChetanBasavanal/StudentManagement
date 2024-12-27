using StudentManagement.API.Models.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DTO
{
    public class TeachersDTO
    {
       
        public int TeacherID { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public int DepartmentID { get; set; }

        //NP
        public Department Department { get; set; }
    }
}
