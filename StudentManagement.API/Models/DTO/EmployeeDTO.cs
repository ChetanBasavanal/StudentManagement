namespace StudentManagement.API.Models.DTO
{
    public class EmployeeDTO
    {
        public int EmployeeID { get; set; }
        public string? EmployeeName { get; set; }

        public int DepartmentID { get; set; }
        public string? Department { get; set; }
    }
}
