using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using System.Security.Claims;

namespace SchoolManagement.Service
{
    public class CourseStudentService : ICourseStudentService
    {
        private readonly SchoolDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseStudentService(SchoolDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private ClaimsPrincipal? CurrentUser => _httpContextAccessor.HttpContext?.User;
        private bool IsAdmin => CurrentUser?.IsInRole("Admin") == true;
        private string? CurrentUserId => CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task AddAsync(CourseStudentDto dto)
        {
            if (dto == null)
                throw new ArgumentException("CourseStudentDto is null.");

            var student = await _context.Students.FindAsync(dto.StudentId);
            if (student == null)
                throw new KeyNotFoundException($"Student with Id {dto.StudentId} not found.");

            var course = await _context.Courses.FindAsync(dto.CourseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with Id {dto.CourseId} not found.");

            var existing = await _context.CourseStudents
                .AnyAsync(cs => cs.StudentId == dto.StudentId && cs.CourseId == dto.CourseId);

            if (existing)
                throw new InvalidOperationException("Student is already registered in this course.");

            if (!IsAdmin)
            {
                if (!int.TryParse(CurrentUserId, out int currentUid))
                    throw new UnauthorizedAccessException("User identification failed.");

                if (course.TeacherId != currentUid)
                    throw new UnauthorizedAccessException("You can only enroll students in your own courses.");
            }

            var entity = new CourseStudent
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId
            };

            await _context.CourseStudents.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(CourseStudentDto dto)
        {
            if (dto == null)
                throw new ArgumentException("CourseStudentDto is null.");

            var entity = await _context.CourseStudents
                .FirstOrDefaultAsync(cs => cs.StudentId == dto.StudentId && cs.CourseId == dto.CourseId);

            if (entity == null)
                throw new KeyNotFoundException("Enrollment not found.");

            if (!IsAdmin)
            {
                if (!int.TryParse(CurrentUserId, out int currentUid))
                    throw new UnauthorizedAccessException("User identification failed.");

                var course = await _context.Courses.FindAsync(dto.CourseId);
                if (course == null || course.TeacherId != currentUid)
                    throw new UnauthorizedAccessException("You can only remove students from your own courses.");
            }

            _context.CourseStudents.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CourseSimpleDto>> GetCoursesByStudentAsync(int studentId)
        {
            if (!IsAdmin)
            {
                if (!int.TryParse(CurrentUserId, out int uid) || uid != studentId)
                    throw new UnauthorizedAccessException("You can only view your own enrolled courses.");
            }

            var courses = await _context.CourseStudents
                .Where(cs => cs.StudentId == studentId)
                .Include(cs => cs.Course)
                .Select(cs => new CourseSimpleDto
                {
                    CourseId = cs.Course.Id,
                    Name = cs.Course.Name,
                    TeacherId = cs.Course.TeacherId
                })
                .ToListAsync();

            return courses;
        }

        public async Task<List<CourseStudentDto>> GetAllAsync()
        {
            if (!IsAdmin)
                throw new UnauthorizedAccessException("Only Admin can view all enrollments.");

            var list = await _context.CourseStudents
                .OrderBy(p => p.CourseId).ThenBy(p => p.StudentId)
                .ToListAsync();

            return await ToDto(list);
        }

        private async Task<List<CourseStudentDto>> ToDto(List<CourseStudent> entities)
        {
            List<CourseStudentDto> dtos = new();

            foreach (var cs in entities)
            {
                dtos.Add(new CourseStudentDto
                {
                    StudentId = cs.StudentId,
                    CourseId = cs.CourseId
                });
            }
            return dtos;
        }
    }
}