using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DTO;
using AutoMapper;
using StudentManagement.API.Models.DomainModels;
using StudentManagement.API.Repositories;
using StudentManagement.API.CustomActionFilter;

namespace StudentManagement.API.Controllers
{
    //localhost:7295/api/Students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentManagementDbContext studentManagementDbContext;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public StudentsController(StudentManagementDbContext studentManagementDbContext, IStudentRepository studentRepository,IMapper mapper)
        {
            this.studentManagementDbContext = studentManagementDbContext;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        //localhost:7295/api/Students
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from DB aka Domain models
            var Students = await studentRepository.GetAllAsync();

            //Map Domain models to DTO's
            var StudentDTO = mapper.Map<List<StudentDTO>>(Students);
            
            return Ok(StudentDTO);
        }

        //GET: localhost:7295/api/Students/{id}
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var Student = await studentRepository.GetByIdAsync(Id); //LINQ
            if (Student != null)
            {
                //Map Data from Domain model to DTO
                var StudentDTOs = mapper.Map<StudentDTO>(Student);
                
                return Ok(StudentDTOs);
            }
            return BadRequest();
        }

        //localhost:7295/api/Students
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddStudentRequestDTO addStudentRequestDTO)
        {
            // map dto to domain model
            var StudentDomainModel =  mapper.Map<Student>(addStudentRequestDTO);
            
            //Use domain model to create Student
            await studentRepository.CreateAsync(StudentDomainModel);
            //map domain model to student
            var StudentDTO = mapper.Map<StudentDTO>(StudentDomainModel);


            return CreatedAtAction(nameof(GetById), new {id=StudentDTO.StudentID}, StudentDTO);
        }

        //POST: localhost:7295/api/Students/{id}
        [HttpPut]
        [Route("{Id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateStudentRequestDTO updateStudentRequestDTO)
        {
            //map DTO to Domain model
            var StudentDomainModel = mapper.Map<Student>(updateStudentRequestDTO);

            if (StudentDomainModel == null)
                return NotFound();

            //update to domain
            await studentRepository.UpdateAsync(Id, StudentDomainModel);
            
            //map domain model to DTO
            var StudentDTO = mapper.Map<StudentDTO>(StudentDomainModel);

            return Ok(StudentDTO);

            
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            //delete data from Domain Model
            var Student = await studentRepository.DeleteAsync(id);

            if(Student == null) return NotFound();

            var StudentDTO = mapper.Map<StudentDTO>(Student);

            return Ok(StudentDTO);
        }
    }
}
