

namespace SchoolManagement.Service
{
    public interface ICourseStudentService
    {
        Task AddAsync(CourseStudentDto dto);
        Task RemoveAsync(CourseStudentDto dto);
        Task<List<CourseSimpleDto>> GetCoursesByStudentAsync(int studentId);
        Task<List<CourseStudentDto>> GetAllAsync();
    }
}