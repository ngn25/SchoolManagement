using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.dto;
using SchoolManagement.Service;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherservice _service;

        public TeacherController(ITeacherservice service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddTeacherDto teacherDto)
        {
            await _service.AddAsync(teacherDto);
            return Ok(teacherDto);
        }
        [HttpGet("GetAll")]
        public async Task<List<TeacherDto>> GetAll()
        {
            return await _service.GetAll();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(TeacherDto teacherDto)
        {
            await _service.UpdateAsync(teacherDto);
            return Ok(teacherDto);
        }
        [HttpGet("GetCourseByEmail")]
        public async Task<List<CourseDto>> GetCoursesByEmail(string Email)
        {
            return await _service.GetCoursesByEmail(Email);
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}