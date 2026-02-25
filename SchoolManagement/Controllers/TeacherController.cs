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
        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddTeacherDto teacherDto)
        {
            await _service.AddAsync(teacherDto);
            return Ok(teacherDto);
        }
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(TeacherDto teacherDto)
        {
            await _service.UpdateAsync(teacherDto);
            return Ok(teacherDto);
        }
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("GetCoursesByEmail")]
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