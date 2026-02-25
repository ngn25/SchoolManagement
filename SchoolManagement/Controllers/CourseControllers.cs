using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using SchoolManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddCourseDto addCourseDto)
        {
            await _service.AddAsync(addCourseDto);
            return Ok(addCourseDto);
        }
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CourseDto courseDto)
        {
            await _service.UpdateAsync(courseDto);
            return Ok(courseDto);
        }
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("GetAll")]
        public async Task<List<CourseDto>> GetAll()
        {
            return await _service.GetAll();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
