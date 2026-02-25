using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Service;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseStudentController : ControllerBase
{
    private readonly ICourseStudentService _service;

    public CourseStudentController(ICourseStudentService service)
    {
        _service = service;
    }
    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost("add")]
    public async Task<IActionResult> AddAsync(CourseStudentDto dto)
    {
        await _service.AddAsync(dto);
        return Ok(new { message = "Student added to course" });
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> RemoveAsync(CourseStudentDto dto)
    {
        await _service.RemoveAsync(dto);
        return Ok(new { message = "Student removed from course" });
    }

    [Authorize(Roles = "Student,Teacher,Admin")]
    [HttpGet("courses/{studentId}")]
    public async Task<IActionResult> GetCoursesByStudentAsync(int studentId)
    {
        var courses = await _service.GetCoursesByStudentAsync(studentId);
        return Ok(courses);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }
}
