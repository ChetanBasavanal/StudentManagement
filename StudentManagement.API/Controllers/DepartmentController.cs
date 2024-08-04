using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.CustomActionFilter;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DomainModels;
using StudentManagement.API.Models.DTO;
using StudentManagement.API.Repositories;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DepartmentController : ControllerBase
    {
        private readonly StudentManagementDbContext studentManagementDbContext;
        private readonly IMapper mapper;
        private readonly IDepartmentRepository departmentRepository;

        public DepartmentController(StudentManagementDbContext studentManagementDbContext, IMapper mapper,IDepartmentRepository departmentRepository)
        {
            this.studentManagementDbContext = studentManagementDbContext;
            this.mapper = mapper;
            this.departmentRepository = departmentRepository;
        }

        //GET:localhost:7295/api/Department
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from Domain Models
            var Department= await departmentRepository.GetAllAsync();

            //Map data from domain model to DTO
            var DepartmentDTO= mapper.Map<List<DepartmentDTO>>(Department);
            //Return to API
            return Ok(DepartmentDTO);
        }

        //localhost:7295/api/Department/{Id}
        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            //Get data from Domain model
            var DepartmentData= await departmentRepository.GetByIdAsync(Id);

            if (DepartmentData != null)
            {
                //map domain model to DTO
                var DepartmentDTO=mapper.Map<DepartmentDTO>(DepartmentData);
                return Ok(DepartmentDTO);
            }
            return BadRequest();
        }

        //localhost:7295/api/Department
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]AddDepartmentDTO addDepartmentDTO)
        {
            //get data from DTO and map to Domain model
            var DomainModel = mapper.Map<Department>(addDepartmentDTO);
            //add domain model to DB
            await departmentRepository.CreateAsync(DomainModel);
            //Map domain model to DTO
            var DepartmentDTO= mapper.Map<DepartmentDTO>(DomainModel);

            return CreatedAtAction(nameof(GetById), new { id = DepartmentDTO.DepartmentID }, DepartmentDTO);
        }

        //localhost:7295/api/Department/{id}
        [HttpPut]
        [Route("{Id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]int Id, [FromBody]UpdateDepartmentDTO updateDepartmentDTO)
        {
            //map data from DTO to Domain model
            var DomainModel = mapper.Map<Department>(updateDepartmentDTO);
            
            if(DomainModel != null)
            {
                //Update data on DB
                await departmentRepository.UpdateAsync(Id, DomainModel);
                
                //map data from Domain model to DTO
                var DepartmentDTO = mapper.Map<DepartmentDTO>(DomainModel);
                return Ok(DepartmentDTO);   
            }
            return BadRequest();
        }

        //localhost:7295/api/Department/{id}
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            //delete data from Domain model
            var DeleteData= await departmentRepository.DeleteAsync(Id);

            if(DeleteData == null)
                return BadRequest();

            //map data to DTO
            var DepartmentDTO = mapper.Map<DepartmentDTO>(DeleteData);
            return Ok(DepartmentDTO);
        }
    }
}
