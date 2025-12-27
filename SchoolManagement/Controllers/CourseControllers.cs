using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.dto;
using SchoolManagement.Service;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController(ICourseService _service) : ControllerBase
    {   
        [HttpPost]
        public IActionResult Add(AddCourseDto AddCourseDto) 
        { 
            _service.Add(AddCourseDto.ToModel()); 
            return Ok(AddCourseDto); 
        }
        
        [HttpPut]
        public IActionResult Update(CourseDto courseDto)
        {
            _service.Update(courseDto.ToModel());
            return Ok(courseDto);
        } 
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
