using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public interface ICourseService
    {
        Task AddAsync(Course course);

        Task UpdateAsync(Course course);

        Task DeleteAsync(int id);
    }
}
