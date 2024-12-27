using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DomainModels
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeID { get; set; }
        public string? EmployeeName { get; set; }

        public int DepartmentID { get; set; }
        public string? Department { get; set; }

        //Navigation Property

        public Department departments { get; set; }
    }
}
