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
        }
    }
}
