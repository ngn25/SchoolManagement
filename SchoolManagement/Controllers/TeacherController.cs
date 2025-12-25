using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.dto;
using SchoolManagement.Service;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController(ITeacherservice _service) : ControllerBase
    {   
        [HttpPost]
        public IActionResult Add(TeacherDto teacherDto) 
        { 
            _service.Add(teacherDto.ToModel()); 
            return Ok(teacherDto); 
        }
        
        [HttpPut]
        public IActionResult Update(TeacherDto teacherDto)
        {
            _service.Update(teacherDto.ToModel());
            return Ok(teacherDto);
        } 
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
