using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;


namespace SchoolManagement.Service
{
    public interface IStudentService
    {
        Task<List<CourseStudentsDto>> GetCoursesByEmail(string Email);
        Task<List<StudentDto>> GetAll();

        Task AddAsync(AddStudentDto student);

        Task UpdateAsync(StudentDto student);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int? id);
    }
}
