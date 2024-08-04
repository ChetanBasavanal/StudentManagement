using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Models.DTO
{
    public class GradesDTO
    {
        public int GradeID { get; set; }

        public int StudentID { get; set; }
        public string GradeGiven { get; set; }
        public string CourseName { get; set; }
        public DateTime DateRecorded { get; set; }

        //Navigational Properties
        public Student Student { get; set; }
    }
}
