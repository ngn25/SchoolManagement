using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public interface ICourseService
    {
        Task AddAsync(AddCourseDto course);

        Task UpdateAsync(CourseDto course);

        Task DeleteAsync(int id);
    }
}
