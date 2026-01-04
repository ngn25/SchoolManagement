using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public class CourseService : ICourseService
    {
        private readonly SchoolDbContext _context;
        private readonly ITeacherservice _teacherService;
        private readonly IStudentService _studentService;

        public CourseService(
            SchoolDbContext context,
            ITeacherservice teacherService,
            IStudentService studentService)
        {
            _context = context;
            _teacherService = teacherService;
            _studentService = studentService;
        }

        public async Task AddAsync(AddCourseDto coursedto)
        {

            if (coursedto == null)
            {
                throw new ArgumentException("coursedto is null .");
            }
            if (!await _teacherService.ExistsAsync(coursedto.TeacherId))
                return;
            Course course = coursedto.ToModel();

            if (!await DoStudentsExistAsync(course))
                return;


            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseDto coursedto)
        {
            if (coursedto == null)
            {
                throw new ArgumentException("coursedto is null.");
            }

            if (!await ExistsAsync(coursedto.Id))
                return;

            if (!await _teacherService.ExistsAsync(coursedto.TeacherId))
                return;
            Course course = coursedto.ToModel();

            if (!await DoStudentsExistAsync(course))
                return;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Courses
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }

        private async Task<bool> ExistsAsync(int? id)
        {
            return await _context.Courses.AnyAsync(c => c.Id == id);
        }

        private async Task<bool> DoStudentsExistAsync(Course course)
        {
            foreach (var cs in course.CourseStudents)
            {
                if (!await _studentService.ExistsAsync(cs.StudentId))
                    return false;
            }
            return true;
        }
    }
}
