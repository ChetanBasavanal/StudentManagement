using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.API.Models.DomainModels
{
    public class Teacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subject { get; set; }
        public int DepartmentID { get; set; }

        //NP
        public Department Department { get; set; }
    }
}
