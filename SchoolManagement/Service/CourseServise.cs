using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{

    public class CourseService : ICourseService
    {
        private readonly SchoolDbContext _context;
        private readonly ITeacherService _teacherService;
        private readonly IStudentService _studentService;

        public CourseService(
            SchoolDbContext context,
            ITeacherService teacherService,
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

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
        public async Task<List<CourseDto>> GetAll()
        {
            var a = await _context.Courses.ToListAsync();

            return await Todto(a);
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

        private async Task<List<CourseDto>> Todto(List<Course> courses)
        {
            List<CourseDto> courseDtos = [];

            foreach (var course in courses)
            {
                CourseDto courseDto = new CourseDto();
                courseDto.Id = course.Id;
                courseDto.Name = course.Name;
                courseDto.TeacherId = course.TeacherId;
                courseDtos.Add(courseDto);
            }
            return courseDtos;
        }

    }
}
