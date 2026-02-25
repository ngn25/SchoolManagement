using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using SchoolManagement.Validation;
using System.Security.Claims;

namespace SchoolManagement.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly SchoolDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeacherService(SchoolDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private ClaimsPrincipal? CurrentUser => _httpContextAccessor.HttpContext?.User;
        private bool IsAdmin => CurrentUser?.IsInRole("Admin") == true;
        private string? CurrentUserId => CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);
        private string? CurrentUserEmail => CurrentUser?.FindFirstValue(ClaimTypes.Email);

        public async Task AddAsync(AddTeacherDto teacherdto)
        {
            if (!IsAdmin)
                throw new UnauthorizedAccessException("Only Admin can add teachers.");

            if (teacherdto == null)
                throw new ArgumentException("teacherdto is null.");

            if (!string.IsNullOrEmpty(teacherdto.Email))
            {
                Validator.ValidateEmail(teacherdto.Email);

                if (await _context.Teachers.AnyAsync(t => t.Email == teacherdto.Email))
                    throw new InvalidOperationException("Teacher with this email already exists.");
            }

            if (!string.IsNullOrEmpty(teacherdto.PhoneNumber))
            {
                Validator.ValidatePhoneNumber(teacherdto.PhoneNumber);

                if (await _context.Teachers.AnyAsync(t => t.PhoneNumber == teacherdto.PhoneNumber))
                    throw new InvalidOperationException("Teacher with this phone number already exists.");
            }

            Teacher teacher = teacherdto.ToModel();
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TeacherDto teacherdto)
        {
            if (!IsAdmin)
                throw new UnauthorizedAccessException("Only Admin can update teachers.");

            if (teacherdto == null)
                throw new ArgumentException("teacherdto is null.");

            if (!await ExistsAsync(teacherdto.Id))
                throw new KeyNotFoundException($"Teacher with ID {teacherdto.Id} not found.");

            if (!string.IsNullOrEmpty(teacherdto.Email))
            {
                Validator.ValidateEmail(teacherdto.Email);

                if (_context.Teachers.Any(p => p.Email == teacherdto.Email && p.Id != teacherdto.Id))
                    throw new ArgumentException("Email already exists among teachers.");

                if (_context.Students.Any(s => s.Email == teacherdto.Email))
                    throw new ArgumentException("Email already exists among students.");
            }

            if (!string.IsNullOrEmpty(teacherdto.PhoneNumber))
            {
                Validator.ValidatePhoneNumber(teacherdto.PhoneNumber);

                if (_context.Teachers.Any(p => p.PhoneNumber == teacherdto.PhoneNumber && p.Id != teacherdto.Id))
                    throw new ArgumentException("Phone number already exists among teachers.");

                if (_context.Students.Any(s => s.PhoneNumber == teacherdto.PhoneNumber))
                    throw new ArgumentException("Phone number already exists among students.");
            }

            Teacher teacher = teacherdto.ToModel();
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CourseDto>> GetCoursesByEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
                throw new ArgumentException("Invalid or empty email.");

            if (!IsAdmin)
            {
                if (string.IsNullOrEmpty(CurrentUserEmail) ||
                    !string.Equals(CurrentUserEmail, Email, StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnauthorizedAccessException("You can only view your own courses.");
                }
            }

            var result = await _context.Courses
                .Where(p => p.Teacher != null && p.Teacher.Email == Email)
                .ToListAsync();

            return await ToDto(result);
        }

        public async Task<List<TeacherDto>> GetAll()
        {
            if (!IsAdmin)
                throw new UnauthorizedAccessException("Only Admin can view all teachers.");

            var teachers = await _context.Teachers.ToListAsync();
            return await Todto(teachers);
        }

        public async Task DeleteAsync(int id)
        {
            if (!IsAdmin)
                throw new UnauthorizedAccessException("Only Admin can delete teachers.");

            await _context.Teachers
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();
        }

        private async Task<List<TeacherDto>> Todto(List<Teacher> teachers)
        {
            List<TeacherDto> dtos = new();

            foreach (var teacher in teachers)
            {
                dtos.Add(new TeacherDto
                {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    Email = teacher.Email,
                    PhoneNumber = teacher.PhoneNumber
                });
            }
            return dtos;
        }

        private async Task<List<CourseDto>> ToDto(List<Course> courses)
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

        public async Task<bool> ExistsAsync(int? id)
        {
            if (!id.HasValue) return false;
            return await _context.Teachers.AnyAsync(t => t.Id == id.Value);
        }
    }
}