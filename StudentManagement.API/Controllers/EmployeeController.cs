using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StudentManagement.API.Caching;
using StudentManagement.API.CustomActionFilter;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DomainModels;
using StudentManagement.API.Models.DTO;
using StudentManagement.API.Repositories;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;
        private readonly ICacheProvider cacheProvider;

        public EmployeeController(IMapper mapper, IEmployeeRepository employeeRepository, ICacheProvider cacheProvider)
        {

            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
            this.cacheProvider = cacheProvider;
        }

        //GET:localhost:7295/api/Employee
        [HttpGet]
        [ValidateModel]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            //get data from DB
            if (!cacheProvider.TryGetValue(CacheKeys.Employee, out List<Employee> EmployeeDomainModel))
            {
                EmployeeDomainModel = await employeeRepository.GetAllAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 10
                };

                cacheProvider.Set(CacheKeys.Employee, EmployeeDomainModel, cacheEntryOptions);

            }
            //map the data from Domain model to DTO
            var EmployeeDTO = mapper.Map<List<Employee>>(EmployeeDomainModel);

            return Ok(EmployeeDTO);
        }

        [HttpGet]
        [Route("{ID:int}")]
        [ValidateModel]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetByID(int EmployeeID)
        {
            //Check whether the data available in DB or not
            var EmployeeData= await employeeRepository.GetByIDAsync(EmployeeID);

            if (EmployeeData == null)
                return NotFound();

            //map data from 
            var EmployeeDTO= mapper.Map<EmployeeDTO>(EmployeeData);

            return Ok(EmployeeDTO);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]AddEmployeeDTO addEmployeeDTO)
        {
            //map data from DTO to domain model
            var EmployeeDomainModel= mapper.Map<Employee>(addEmployeeDTO);

            //create data in DB
            EmployeeDomainModel = await employeeRepository.CreateAsync(EmployeeDomainModel);

                //map back domain model to DTO
                var EmployeeDTO = mapper.Map<EmployeeDTO>(EmployeeDomainModel);
            return Ok(EmployeeDTO);

        }

        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]int Id, [FromBody]UpdateEmployeeDTO updateEmployeeDTO)
        {
            //map data from DTO to domain model.
            var EmployeeDomainModel = mapper.Map<Employee>(updateEmployeeDTO);

            //Checking EMployee exist or not has done at backend.
            EmployeeDomainModel= await employeeRepository.UpdateAsync(Id,EmployeeDomainModel);

            if (EmployeeDomainModel == null)
            {
                return NotFound();
            }
            //map data back from Domain model to DTO.
            var EmployeeDTO = mapper.Map<EmployeeDTO>(updateEmployeeDTO);

            return Ok(EmployeeDTO);

        }

        [HttpDelete]
        [Route("{ID:int}")]
        [ValidateModel]
        public async Task<IActionResult> Delete([FromRoute]int ID)
        {
            var EmployeeDomainModel= await employeeRepository.DeleteAsync(ID);

            if (EmployeeDomainModel == null)
                return NotFound();

            //map data from domain model to DTO and return to Endpoint.
            return Ok(mapper.Map<EmployeeDTO>(EmployeeDomainModel));
        }
    }
}
