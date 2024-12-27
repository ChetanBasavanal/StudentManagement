using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StudentManagement.API.Caching;
using StudentManagement.API.CustomActionFilter;
using StudentManagement.API.Models.DomainModels;
using StudentManagement.API.Models.DTO;
using StudentManagement.API.Repositories;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITeacherRespository teacherRespository;
        private readonly ICacheProvider cacheProvider;

        public TeachersController(IMapper mapper,ITeacherRespository teacherRespository,ICacheProvider cacheProvider)
        {
            this.mapper = mapper;
            this.teacherRespository = teacherRespository;
            this.cacheProvider = cacheProvider;
        }

        [HttpGet]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            //get data from Domain model
            if(!cacheProvider.TryGetValue(CacheKeys.Teachers,out List<Teacher> TeacherDomainModel))
            {
                TeacherDomainModel= await teacherRespository.GetAllAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration=TimeSpan.FromSeconds(30),
                    Size=1024
                };
                cacheProvider.Set(CacheKeys.Teachers,TeacherDomainModel,cacheEntryOptions); 

            }
            //In the above code Inmemory Caching in implemented

            if (TeacherDomainModel == null)
                return BadRequest();

            //map data to DTO from domain model
            var TeacherDTO= mapper.Map<List<TeachersDTO>>(TeacherDomainModel);
            return Ok(TeacherDTO);
        }

        [HttpGet]
        [Route("{Id:int}")]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var DomainData= await teacherRespository.GetByIdAsync(Id);

            if(DomainData == null)
                return BadRequest();

            var TeachersDTO= mapper.Map<TeachersDTO>(DomainData);
            return Ok(TeachersDTO);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles ="Writer")] //if you want to add both roles, you just need to remove the roles from this line or, (Roles="Writer,Reader")
        public async Task<IActionResult> Create([FromBody]AddTeacherDTO addTeacherDTO)
        {
            //map data from DTO to domain model
            var DomainData = mapper.Map<Teacher>(addTeacherDTO);

            if (DomainData != null)
            {
                //add data to DB via Domain model
                await teacherRespository.CreateAsync(DomainData);
                //map data to DTO from Domain model
                var TeacherDTO = mapper.Map<TeachersDTO>(DomainData);

                return CreatedAtAction(nameof(GetById), new { id = TeacherDTO.TeacherID }, TeacherDTO);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{Id:int}")]
        [ValidateModel]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Update([FromRoute]int Id, [FromBody] UpdateTeachersDTO updateTeachersDTO)
        {
            //map data from DTO to Domain model
            var TeacherDomainModel = mapper.Map<Teacher>(updateTeachersDTO);
            if(TeacherDomainModel == null)
                return BadRequest();

            //update data from domain model to DB
            await teacherRespository.UpdateAsync(Id, TeacherDomainModel);
            //map domain model to DTO
            var TeacherDTO = mapper.Map<TeachersDTO>(TeacherDomainModel);

            return CreatedAtAction(nameof(GetById),new {Id=TeacherDTO.TeacherID}, TeacherDTO);
        }


        [HttpDelete]
        [Route("{Id:int}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Delete([FromRoute]int Id)
        {
            //delete data from Domain model
            var DeletedData= await teacherRespository.DeleteAsync(Id);

            if(DeletedData == null)
                return BadRequest();

            //mapping data to DTO
            var TeacherDTO= mapper.Map<TeachersDTO>(DeletedData);
            return Ok(TeacherDTO);
        }
    }
}
