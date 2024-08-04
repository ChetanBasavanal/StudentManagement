namespace StudentManagement.API.Models.DTO
{
    public class UpdateGradeDTO
    {
        public int? StudentID { get; set; }
        public string? GradeGiven { get; set; }
        public string? CourseName { get; set; }
        public DateTime? DateRecorded { get; set; }
    }
}
