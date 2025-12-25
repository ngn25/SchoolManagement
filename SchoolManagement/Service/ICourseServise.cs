using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public interface ICourseService
    {
        void Add(Course course);

        void Update(Course course);

        void Delete(int Id);
    }
}
