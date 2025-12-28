using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Service;
using SchoolManagement.Domain.dto;

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

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentDto studentDto)
        {
            await _service.AddAsync(studentDto.ToModel());
            return Ok(studentDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(StudentDto studentDto)
        {
            await _service.UpdateAsync(studentDto.ToModel());
            return Ok(studentDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
