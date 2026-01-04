using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public interface ITeacherservice
    {
        Task AddAsync(AddTeacherDto teacher);

        Task UpdateAsync(TeacherDto teacher);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int? id);
    }
}
