using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.API.Models.DomainModels
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentID { get; set; }

        [Required]
        public string DepartmentName { get; set; }
        public string Head { get; set; }


        //Navigational properties

        public ICollection<Employee> employees { get; set; }
    }
}
