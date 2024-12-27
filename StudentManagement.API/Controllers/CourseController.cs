using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using StudentManagement.API.Caching;
using StudentManagement.API.CustomActionFilter;
using StudentManagement.API.Models.DomainModels;
using StudentManagement.API.Models.DTO;
using StudentManagement.API.Repositories;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICourseRepository courseRepository;
        private readonly ICacheProvider cacheProvider;

        public CourseController(IMapper mapper, ICourseRepository courseRepository, ICacheProvider cacheProvider)
        {
            this.mapper = mapper;
            this.courseRepository = courseRepository;
            this.cacheProvider = cacheProvider;
        }

        [HttpGet]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            if (!cacheProvider.TryGetValue(CacheKeys.Course, out List<Course> courseDomainModel))
            {
                courseDomainModel = await courseRepository.GetAllAsync();
                var CacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 10
                };
                cacheProvider.Set(CacheKeys.Course, courseDomainModel, CacheEntryOptions);

            }
            // in the above we implemented In-Memory Caching.
            if (courseDomainModel == null)
            {
                return BadRequest();
            }
            var CourseDto = mapper.Map<List<CourseDTO>>(courseDomainModel);

            return Ok(CourseDto);
        }


        [HttpGet]
        [Route("{Id:int}")]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var courseData = await courseRepository.GetByIdAsync(Id);
            if (!cacheProvider.TryGetValue(CacheKeys.Course, out Course courseDomainModel))
            {

                var CacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 10
                };
                cacheProvider.Set(CacheKeys.Course, courseData, CacheEntryOptions);
            }

            if (courseData == null)
            {
                return BadRequest();
            }
            var CourseDto = mapper.Map<CourseDTO>(courseData);
            return Ok(CourseDto);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Add([FromBody] AddCourseDTO addCourseDTO)
        {
            //map data from Dtot to Domain model
            var CourseDomainData = mapper.Map<Course>(addCourseDTO);

            if (CourseDomainData == null)
            {
                return BadRequest();
            }
            //add data to DB
            await courseRepository.CreateAsync(CourseDomainData);
            //map Domain model data to DTO
            var CourseDTO = mapper.Map<CourseDTO>(CourseDomainData);
            return CreatedAtAction(nameof(GetById), new { id = CourseDTO.CourseID }, CourseDTO);
        }

        [HttpPut]
        [Route("{Id:int}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Update([FromRoute]int Id, [FromBody]UpdateCourseDTO updateCourseDTO)
        {
            //map data from DTO to domain model
            var CourseDomainModel= mapper.Map<Course>(updateCourseDTO);

            if(CourseDomainModel == null)
                return BadRequest();
            //update data to DB
            await courseRepository.UpdateAsync(Id, CourseDomainModel);
            var CourseDTO= mapper.Map<CourseDTO>(CourseDomainModel);
            return Ok(CourseDTO);
            
        }

        [HttpDelete]
        [Route("{Id:int}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Delete([FromRoute]int Id)
        {
            var DeleteData= await courseRepository.DeleteAsync(Id);

            if(DeleteData == null) return BadRequest();

            var CourseDTO = mapper.Map<CourseDTO>(DeleteData);
            return Ok(CourseDTO);
        }
    }
}
