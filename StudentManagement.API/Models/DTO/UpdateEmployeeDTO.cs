namespace StudentManagement.API.Models.DTO
{
    public class UpdateEmployeeDTO
    {
        public string? EmployeeName { get; set; }

        public int DepartmentID { get; set; }
        public string? Department { get; set; }
    }
}
