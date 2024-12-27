namespace StudentManagement.UI.Models
{
    public class AddStudentViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
    }
}
