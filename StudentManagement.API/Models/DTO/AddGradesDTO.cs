using StudentManagement.API.Models.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DTO
{
    public class AddGradesDTO
    {
        [Required]
        public int StudentID { get; set; }
        [Required]
        public string GradeGiven { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public DateTime DateRecorded { get; set; }

    }
}
