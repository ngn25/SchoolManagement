using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using SchoolManagement.Service;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddCourseDto addCourseDto)
        {
            await _service.AddAsync(addCourseDto);
            return Ok(addCourseDto);
        }
        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CourseDto courseDto)
        {
            await _service.UpdateAsync(courseDto);
            return Ok(courseDto);
        }
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<List<CourseDto>> GetAll()
        {
            return await _service.GetAll();
        }
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
