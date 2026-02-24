using SchoolManagement.Domain.dto;


namespace SchoolManagement.Service
{
    public interface ITeacherService
    {
        Task<List<CourseDto>> GetCoursesByEmail(string Email);
        Task<List<TeacherDto>> GetAll();
        Task AddAsync(AddTeacherDto teacher);

        Task UpdateAsync(TeacherDto teacher);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int? id);
    }
}
