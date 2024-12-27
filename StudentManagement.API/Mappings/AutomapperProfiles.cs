using AutoMapper;
using StudentManagement.API.Models.DomainModels;
using StudentManagement.API.Models.DTO;

namespace StudentManagement.API.Mappings
{
    public class AutomapperProfiles:Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Student,StudentDTO>().ReverseMap();
            CreateMap<AddStudentRequestDTO,Student >().ReverseMap();
            CreateMap<UpdateStudentRequestDTO,Student>().ReverseMap();
            CreateMap<Department,DepartmentDTO>().ReverseMap();
            CreateMap<AddDepartmentDTO,Department>().ReverseMap();
            CreateMap<UpdateDepartmentDTO, Department>().ReverseMap();
            CreateMap<Grade, GradeDTO>().ReverseMap();
            CreateMap<AddGradesDTO,Grade>().ReverseMap();
            CreateMap<UpdateGradeDTO, Grade>().ReverseMap();
            CreateMap<Teacher,TeachersDTO>().ReverseMap();
            CreateMap<AddTeacherDTO,Teacher>().ReverseMap();
            CreateMap<UpdateTeachersDTO,Teacher>().ReverseMap();
            CreateMap<Course,CourseDTO>().ReverseMap(); 
            CreateMap<AddCourseDTO,Course>().ReverseMap();
            CreateMap<UpdateCourseDTO,Course>().ReverseMap();
            CreateMap<EmployeeDTO,Employee>().ReverseMap();
            CreateMap<AddEmployeeDTO,Employee>().ReverseMap();
            CreateMap<UpdateEmployeeDTO,Employee>().ReverseMap();
           // CreateMap<Employee,EmployeeDTO>().ReverseMap();
        }
    }
}
