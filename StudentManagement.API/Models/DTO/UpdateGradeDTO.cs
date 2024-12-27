using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DTO
{
    public class UpdateGradeDTO
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
