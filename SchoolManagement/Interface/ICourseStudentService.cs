using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolManagement.Domain.Model;

public interface ICourseStudentService
{
    Task AddAsync(CourseStudentDto dto);
    Task RemoveAsync(CourseStudentDto dto);
    Task<List<Course>> GetCoursesByStudentAsync(int studentId);
    Task<List<CourseStudentDto>> GetAllAsync();
}
