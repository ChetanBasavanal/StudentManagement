using AutoMapper;
using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.JSInterop;
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

        public GradesController(IMapper mapper,IGradeRepository gradeRepository)
        {
            this.mapper = mapper;
            this.gradeRepository = gradeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from domain model
            var GradeDomainData= await gradeRepository.GetAllAsync();    

            if(GradeDomainData==null)
                return NotFound();

            //map data from domain model to DTO
            var GradesDTO= mapper.Map<List<GradesDTO>>(GradeDomainData);
            return Ok(GradesDTO);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int Id)
        {
            //get data from Domain model
            var GradeDomainModel= await gradeRepository.GetByIdAsync(Id);

            if(GradeDomainModel==null) 
                return NotFound();

            //map data to DTO
            var GradeDTO = mapper.Map<GradesDTO>(GradeDomainModel);

            return Ok(GradeDTO);
            
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]AddGradeDTO addGradeDTO)
        {
            //map data from DTO to Domain model
            var GradeDomainModel= mapper.Map<Grade>(addGradeDTO);
            if(GradeDomainModel == null)
                return BadRequest();

            //add data to DB
            await gradeRepository.CreateAsync(GradeDomainModel);
            //map data to Domain model to DTO
            var GradeDTO = mapper.Map<GradesDTO>(GradeDomainModel);

            return CreatedAtAction(nameof(GetById), new {id=GradeDTO.GradeID},GradeDTO);  //it produces status code of 201 instead of 200, which means data is added successfully.
        }

        [HttpPut]
        [Route("{Id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]int Id, [FromBody]UpdateGradeDTO updateGradeDTO)
        {
            //map data from DTO to Domain model
            var GradeDomainModel= mapper.Map<Grade>(updateGradeDTO);
            
            if(GradeDomainModel != null)
            {
                //Update Data from domain model to DTO
                await gradeRepository.UpdateAsync(Id, GradeDomainModel);
                //map data from Domain model to DTO
                var GradeDTO = mapper.Map<GradesDTO>(GradeDomainModel);

                return Ok(GradeDTO);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            //Delete data from Domain model
            var GradeDataToDelete= await gradeRepository.DeleteAsync(Id);   
            if(GradeDataToDelete != null)
            {
                //map data from domain model to DTO
                var GradeDTO = mapper.Map<GradesDTO>(GradeDataToDelete);
                return Ok(GradeDTO);
            }
            return BadRequest();
        }


    }
}
