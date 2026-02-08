using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Service;
using SchoolManagement.Domain.dto;
using Microsoft.AspNetCore.Authorization;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddStudentDto studentDto)
        {
            await _service.AddAsync(studentDto);
            return Ok(studentDto);
        }
        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(StudentDto studentDto)
        {
            await _service.UpdateAsync(studentDto);
            return Ok(studentDto);
        }
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<List<StudentDto>> GetAll()
        {
            return await _service.GetAll();
        }
        [Authorize]
        [HttpGet("GetCourseByEmail")]
        public async Task<List<CourseStudentDto>> GetCoursesByEmail(string Email)
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
