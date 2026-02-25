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
        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddStudentDto studentDto)
        {
            await _service.AddAsync(studentDto);
            return Ok(studentDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(StudentDto studentDto)
        {
            await _service.UpdateAsync(studentDto);
            return Ok(studentDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("GetCourseByEmail")]
        public async Task<IActionResult> GetCoursesByEmail(string Email)
        {
            var result = await _service.GetCoursesByEmail(Email);
            return Ok(result);
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
