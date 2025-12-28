using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public interface IStudentService
    {
        Task AddAsync(Student student);

        Task UpdateAsync(Student student);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int? id);
    }
}
