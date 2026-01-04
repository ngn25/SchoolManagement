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

        [HttpPost]
        public async Task<IActionResult> Add(AddTeacherDto teacherDto)
        {
            await _service.AddAsync(teacherDto);
            return Ok(teacherDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(TeacherDto teacherDto)
        {
            await _service.UpdateAsync(teacherDto);
            return Ok(teacherDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}