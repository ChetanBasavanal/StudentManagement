using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Caching.Memory;
using StudentManagement.API.Caching;
using StudentManagement.API.CustomActionFilter;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DomainModels;
using StudentManagement.API.Models.DTO;
using StudentManagement.API.Repositories;
using System.Text.Json;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly StudentManagementDbContext studentManagementDbContext;
        private readonly IMapper mapper;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ICacheProvider cacheProvider;
        private readonly ILogger<DepartmentController> logger;

        public DepartmentController(StudentManagementDbContext studentManagementDbContext, IMapper mapper,
            IDepartmentRepository departmentRepository,ICacheProvider cacheProvider,ILogger<DepartmentController> logger)
        {
            this.studentManagementDbContext = studentManagementDbContext;
            this.mapper = mapper;
            this.departmentRepository = departmentRepository;
            this.cacheProvider = cacheProvider;
            this.logger = logger;
        }

        //GET:localhost:7295/api/Department
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
                //Get data from Domain Models
                if (!cacheProvider.TryGetValue(CacheKeys.Departments, out List<Department> Department))
                {
                    //logger.LogInformation("GetAll Department Controller has been invoked");

                    Department = await departmentRepository.GetAllAsync();

                   // logger.LogInformation($"Finished GetAll departmentController request:{JsonSerializer.Serialize(Department)}");

                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                        SlidingExpiration = TimeSpan.FromSeconds(30),
                        Size = 1024
                    };
                    cacheProvider.Set(CacheKeys.Departments, Department, cacheEntryOptions);
                }
            //In the above code In-memory Chacing implemented

            //Create an new exception for testing
            //throw new Exception("This is new exception for testing");

                //Map data from domain model to DTO
                var DepartmentDTO = mapper.Map<List<DepartmentDTO>>(Department);
                //Return to API
                return Ok(DepartmentDTO);
           
        }

        //localhost:7295/api/Department/{Id}
        [HttpGet]
        [Route("{Id:int}")]
        //[Authorize(Roles ="Reader")]
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
        //[Authorize(Roles ="Writer")] //if you want to add both roles, you just need to remove the roles from this line or, (Roles="Writer,Reader")
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
        //[Authorize(Roles ="Writer")]
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
        //[Authorize(Roles ="Writer")]
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
