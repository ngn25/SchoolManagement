using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using SchoolManagement.Validation;
using System.Security.Claims;

namespace SchoolManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly SchoolDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentService(SchoolDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private ClaimsPrincipal? CurrentUser => _httpContextAccessor.HttpContext?.User;
        private bool IsAdmin => CurrentUser?.IsInRole("Admin") == true;
        private string? CurrentUserId => CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);
        private string? CurrentUserEmail => CurrentUser?.FindFirstValue(ClaimTypes.Email);

        public async Task AddAsync(AddStudentDto studentdto)
        {
            if (!IsAdmin)
                throw new UnauthorizedAccessException("Only Admin can add students.");

            if (studentdto == null)
                throw new ArgumentException("studentdto is null.");

            if (!string.IsNullOrEmpty(studentdto.Email))
                Validator.ValidateEmail(studentdto.Email);

            if (await _context.Students.AnyAsync(p => p.Email == studentdto.Email))
                throw new ArgumentException("Email already exists among students.");

            if (await _context.Teachers.AnyAsync(p => p.Email == studentdto.Email))
                throw new ArgumentException("Email already exists among teachers.");

            if (!string.IsNullOrEmpty(studentdto.PhoneNumber))
                Validator.ValidatePhoneNumber(studentdto.PhoneNumber);

            if (await _context.Students.AnyAsync(p => p.PhoneNumber == studentdto.PhoneNumber))
                throw new ArgumentException("Phone number already exists among students.");

            if (await _context.Teachers.AnyAsync(p => p.PhoneNumber == studentdto.PhoneNumber))
                throw new ArgumentException("Phone number already exists among teachers.");

            Student student = studentdto.ToModel();
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentDto studentdto)
        {
            if (!IsAdmin)
                throw new UnauthorizedAccessException("Only Admin can update students.");

            if (studentdto == null)
                throw new ArgumentException("studentdto is null.");

            if (!await ExistsAsync(studentdto.Id))
                throw new KeyNotFoundException($"Student with ID {studentdto.Id} not found.");

            if (!string.IsNullOrEmpty(studentdto.Email))
            {
                Validator.ValidateEmail(studentdto.Email);

                if (_context.Teachers.Any(p => p.Email == studentdto.Email))
                    throw new ArgumentException("Email already exists among teachers.");

                if (_context.Students.Any(s => s.Email == studentdto.Email && s.Id != studentdto.Id))
                    throw new ArgumentException("Email already exists among students.");
            }

            if (!string.IsNullOrEmpty(studentdto.PhoneNumber))
            {
                Validator.ValidatePhoneNumber(studentdto.PhoneNumber);

                if (_context.Teachers.Any(p => p.PhoneNumber == studentdto.PhoneNumber))
                    throw new ArgumentException("Phone number already exists among teachers.");

                if (_context.Students.Any(s => s.PhoneNumber == studentdto.PhoneNumber && s.Id != studentdto.Id))
                    throw new ArgumentException("Phone number already exists among students.");
            }

            Student student = studentdto.ToModel();
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentDto>> GetAll()
        {
            if (!IsAdmin)
                throw new UnauthorizedAccessException("Only Admin can view all students.");

            var students = await _context.Students.ToListAsync();
            return await Todto(students);
        }

        public async Task<List<CourseStudentsDto>> GetCoursesByEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
                throw new ArgumentException("Invalid or empty email.");

            if (!IsAdmin)
            {
                if (string.IsNullOrEmpty(CurrentUserEmail) ||
                    !string.Equals(CurrentUserEmail, Email, StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnauthorizedAccessException("You can only view your own enrolled courses.");
                }
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(p => p.Email == Email);

            if (student == null)
                return new List<CourseStudentsDto>();

            var result = await _context.CourseStudents
                .Include(p => p.Course)
                .Where(p => p.StudentId == student.Id)
                .ToListAsync();

            return await ToDto(result);
        }

        public async Task DeleteAsync(int id)
        {
            if (!IsAdmin)
                throw new UnauthorizedAccessException("Only Admin can delete students.");

            await _context.Students
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
        }

        private async Task<List<StudentDto>> Todto(List<Student> students)
        {
            List<StudentDto> studentDtos = new();

            foreach (var student in students)
            {
                studentDtos.Add(new StudentDto
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber,
                    DateOfBirth = student.DateOfBirth
                });
            }
            return studentDtos;
        }

        private async Task<List<CourseStudentsDto>> ToDto(List<CourseStudent> courseStudents)
        {
            List<CourseStudentsDto> dtos = new();

            foreach (var cs in courseStudents)
            {
                dtos.Add(new CourseStudentsDto
                {
                    CourseId = cs.CourseId,
                    CourseName = cs.Course?.Name ?? "Unknown"
                });
            }
            return dtos;
        }

        public async Task<bool> ExistsAsync(int? id)
        {
            if (!id.HasValue) return false;
            return await _context.Students.AnyAsync(s => s.Id == id.Value);
        }
    }
}