using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.API.Models.DomainModels
{
    public class Grade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GradeID { get; set; }

        public int? StudentID { get; set; }
        public string? GradeGiven { get; set; }
        public string? CourseName { get; set; }
        public DateTime? DateRecorded { get; set; }

        //Navigational Properties
        public Student? Student { get; set; }
    }
}
