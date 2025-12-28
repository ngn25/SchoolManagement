using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public interface ITeacherservice
    {
        Task AddAsync(Teacher teacher);

        Task UpdateAsync(Teacher teacher);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int? id);
    }
}
