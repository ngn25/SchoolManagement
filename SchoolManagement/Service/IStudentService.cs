using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public interface IStudentService
    {
        void Add(Student student);

        void Update(Student student);

        void Delete(int Id);
    }
}
