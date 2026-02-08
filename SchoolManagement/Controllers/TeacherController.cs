using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.dto;
using SchoolManagement.Service;
using Microsoft.AspNetCore.Authorization;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _service;

        public TeacherController(ITeacherService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddTeacherDto teacherDto)
        {
            await _service.AddAsync(teacherDto);
            return Ok(teacherDto);
        }
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<List<TeacherDto>> GetAll()
        {
            return await _service.GetAll();
        }
        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(TeacherDto teacherDto)
        {
            await _service.UpdateAsync(teacherDto);
            return Ok(teacherDto);
        }
        [Authorize]
        [HttpGet("GetCoursesByEmail")]
        public async Task<List<CourseDto>> GetCoursesByEmail(string Email)
        {
            return await _service.GetCoursesByEmail(Email);
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