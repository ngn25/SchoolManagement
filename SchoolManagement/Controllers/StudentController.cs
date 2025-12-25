using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Service;
using SchoolManagement.Domain.Model;
using SchoolManagement.Domain.dto;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController(IStudentService _service) : ControllerBase
    {
        [HttpPost]
        public IActionResult Add(StudentDto studentDto) 
        { 
            _service.Add(studentDto.ToModel()); 
            return Ok(studentDto); 
        }
        
        [HttpPut]
        public IActionResult Update(StudentDto studentDto)
        {
            _service.Update(studentDto.ToModel());
            return Ok(studentDto);
        } 
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
