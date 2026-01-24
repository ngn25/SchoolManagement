using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using SchoolManagement.Service;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddCourseDto addCourseDto)
        {
            await _service.AddAsync(addCourseDto);
            return Ok(addCourseDto);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CourseDto courseDto)
        {
            await _service.UpdateAsync(courseDto);
            return Ok(courseDto);
        }

        [HttpGet("GetAll")]
        public async Task<List<CourseDto>> GetAll()
        {
            return await _service.GetAll();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
