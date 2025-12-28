using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.dto;
using SchoolManagement.Service;

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

        [HttpPost]
        public async Task<IActionResult> Add(AddCourseDto addCourseDto)
        {
            await _service.AddAsync(addCourseDto.ToModel());
            return Ok(addCourseDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseDto courseDto)
        {
            await _service.UpdateAsync(courseDto.ToModel());
            return Ok(courseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
