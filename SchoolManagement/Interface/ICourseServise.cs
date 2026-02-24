using SchoolManagement.Domain.dto;


namespace SchoolManagement.Service
{
    public interface ICourseService
    {
        Task <List<CourseDto>> GetAll();
        Task AddAsync(AddCourseDto course);

        Task UpdateAsync(CourseDto course);

        Task DeleteAsync(int id);
    }
}
