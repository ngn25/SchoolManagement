using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using System.Security.Claims;

namespace SchoolManagement.Service
{
    public class CourseService : ICourseService
    {
        private readonly SchoolDbContext _context;
        private readonly ITeacherService _teacherService;
        private readonly IStudentService _studentService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseService(
            SchoolDbContext context,
            ITeacherService teacherService,
            IStudentService studentService,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _teacherService = teacherService;
            _studentService = studentService;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private ClaimsPrincipal? CurrentUser => _httpContextAccessor.HttpContext?.User;
        private bool IsAdmin => CurrentUser?.IsInRole("Admin") == true;
        private string? CurrentUserId => CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task AddAsync(AddCourseDto coursedto)
        {
            if (coursedto == null)
                throw new ArgumentException("coursedto is null.");

            if (!await _teacherService.ExistsAsync(coursedto.TeacherId))
                throw new KeyNotFoundException($"Teacher with ID {coursedto.TeacherId} not found.");

            if (await _context.Courses.AnyAsync(c =>
                 c.Name == coursedto.Name &&
                 c.TeacherId == coursedto.TeacherId))
                throw new ArgumentException("This teacher already has a course with the same name.");

            if (!IsAdmin)
            {
                if (!int.TryParse(CurrentUserId, out int uid) || coursedto.TeacherId != uid)
                    throw new UnauthorizedAccessException("You can only create courses for yourself.");
            }

            Course course = coursedto.ToModel();
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseDto coursedto)
        {
            if (coursedto == null)
                throw new ArgumentException("coursedto is null.");

            if (!await ExistsAsync(coursedto.Id))
                throw new KeyNotFoundException($"Course with ID {coursedto.Id} not found.");

            if (!await _teacherService.ExistsAsync(coursedto.TeacherId))
                throw new KeyNotFoundException($"Teacher with ID {coursedto.TeacherId} does not exist.");

            if (!IsAdmin)
            {
                if (!int.TryParse(CurrentUserId, out int uid) || coursedto.TeacherId != uid)
                    throw new UnauthorizedAccessException("You can only update your own courses.");
            }

            Course course = coursedto.ToModel();
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CourseDto>> GetAll()
        {
            // می‌تونی تصمیم بگیری همه ببینن یا فقط ادمین/استادها
            var courses = await _context.Courses.ToListAsync();
            return await Todto(courses);
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            if (!IsAdmin)
            {
                if (!int.TryParse(CurrentUserId, out int uid) || course.TeacherId != uid)
                    throw new UnauthorizedAccessException("You can only delete your own courses.");
            }

            await _context.Courses
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }

        private async Task<bool> ExistsAsync(int? id)
        {
            if (!id.HasValue) return false;
            return await _context.Courses.AnyAsync(c => c.Id == id.Value);
        }

        private async Task<List<CourseDto>> Todto(List<Course> courses)
        {
            List<CourseDto> dtos = new();

            foreach (var course in courses)
            {
                dtos.Add(new CourseDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    TeacherId = course.TeacherId
                });
            }
            return dtos;
        }
    }
}