using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    
    public class GradesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGradeRepository gradeRepository;
        private readonly ICacheProvider cacheProvider;

        public GradesController(IMapper mapper,IGradeRepository gradeRepository,ICacheProvider cacheProvider)
        {
            this.mapper = mapper;
            this.gradeRepository = gradeRepository;
            this.cacheProvider = cacheProvider;
        }

        [HttpGet]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            //Get data from Domain model
            if (!cacheProvider.TryGetValue(CacheKeys.Grades,out List<Grade> gradesDomainModel))
            {
                gradesDomainModel=await gradeRepository.GetAllAsync();
                var cacheEntryoptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 10
                };

                cacheProvider.Set(CacheKeys.Grades,gradesDomainModel,cacheEntryoptions);
            }

            //In the above code In-memory Caching in implemented.

            if (gradesDomainModel == null)
            {
                return BadRequest();
            }
            var GradeDTO = mapper.Map<List<GradeDTO>>(gradesDomainModel);
            //map data from domain model to DTO
            return Ok(GradeDTO);
        }

        [HttpGet]
        [Route("{Id:int}")]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetByID([FromRoute] int Id)
        {
            //get data from Domain model via ID
            var GradeData= await gradeRepository.GetByIdAsync(Id);

            if(GradeData == null)
                return BadRequest();

            //map Data from DTO
            var GradeDTO= mapper.Map<GradeDTO>(GradeData);
            return Ok(GradeDTO);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles ="Writer")] //if you want to add both roles, you just need to remove the roles from this line or, (Roles="Writer,Reader")
        public async Task<IActionResult> Create([FromBody] AddGradesDTO addGradesDTO)
        {
            //Map data from Dto to Domain models
            var GradeDomainData= mapper.Map<Grade>(addGradesDTO);

            if(GradeDomainData == null)
                return BadRequest();

            //add data to Db
            await gradeRepository.CreateAsync(GradeDomainData);
            //map data back to DTO
            var GradeDTO = mapper.Map<GradeDTO>(GradeDomainData);

            return CreatedAtAction(nameof(GetByID), new {id=GradeDTO.GradeID}, GradeDTO);   
        }

        [HttpPut]
        [Route("{Id:int}")]
        [ValidateModel]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Update([FromRoute] int? Id, [FromBody]UpdateGradeDTO updateGradeDTO)
        {
            //get data from DTO to domain model
            var GradeDomainModel = mapper.Map<Grade>(updateGradeDTO);

            if(GradeDomainModel == null)
                return BadRequest();

            //update data in DB
            await gradeRepository.UpdateAsync(Id, GradeDomainModel);    
            //map domain model to DTO
            var GradeDTO= mapper.Map<GradeDTO>(GradeDomainModel);

            return CreatedAtAction(nameof (GetByID), new { Id = GradeDTO.GradeID },GradeDTO);
        }

        [HttpDelete]
        [Route("{Id:int}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            //delete data from DB
            var DeleteData= await gradeRepository.DeleteAsync(Id);

            if(DeleteData == null) return BadRequest();

            //Map to DTO from domain model
            var GradeDTO= mapper.Map<GradeDTO>(DeleteData);

            return Ok(GradeDTO);
        }
    }
}
