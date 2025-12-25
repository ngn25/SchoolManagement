using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public interface ITeacherservice
    {
        void Add(Teacher teacher);

        void Update(Teacher teacher);

        void Delete(int Id);
    }
}
