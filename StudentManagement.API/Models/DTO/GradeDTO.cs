using StudentManagement.API.Models.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DTO
{
    public class GradeDTO
    {
        public int GradeID { get; set; }

        [Required]
        public int StudentID { get; set; }
        [Required]
        public string GradeGiven { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public DateTime DateRecorded { get; set; }

        //Navigational Properties
        public Student Student { get; set; }
        
    }
}
