using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using SchoolManagement.Validation;
using Microsoft.AspNetCore.Authorization;

namespace SchoolManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly SchoolDbContext _context;

        public StudentService(SchoolDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task AddAsync(AddStudentDto studentdto)
        {
            if (studentdto == null)
            {
                throw new ArgumentException("studentdto is null.");
            }
            ;


            if (!string.IsNullOrEmpty(studentdto.Email))
                Validator.ValidateEmail(studentdto.Email);
            if (_context.Teachers.Any(p => p.Email == studentdto.Email))
            {
                throw new ArgumentException("already exist .");
            }

            if (!string.IsNullOrEmpty(studentdto.PhoneNumber))
                Validator.ValidatePhoneNumber(studentdto.PhoneNumber);
            if (_context.Teachers.Any(p => p.PhoneNumber == studentdto.PhoneNumber))
            {
                throw new ArgumentException("already exist .");
            }


            Student student = studentdto.ToModel();

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }
        [Authorize]
        public async Task UpdateAsync(StudentDto studentdto)
        {
            if (studentdto == null)
            {
                throw new ArgumentException("studentdto is null.");
            }
            ;

            if (!await ExistsAsync(studentdto.Id))
                return;

            if (!string.IsNullOrEmpty(studentdto.Email))
                Validator.ValidateEmail(studentdto.Email);

            if (!string.IsNullOrEmpty(studentdto.PhoneNumber))
                Validator.ValidatePhoneNumber(studentdto.PhoneNumber);

            Student student = studentdto.ToModel();

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

    
        public async Task<List<StudentDto>> GetAll()
        {
            var a = await _context.Students.ToListAsync();

            return await Todto(a);
        }

               public async Task<List<CourseStudentsDto>> GetCoursesByEmail(string Email)

        {
            if (!string.IsNullOrEmpty(Email))
                Validator.ValidateEmail(Email);

            var studentIds = _context.Students.FirstOrDefault(p => p.Email == Email);
            var result = await _context.CourseStudents.Include(p => p.Course).Where(p => p.StudentId == studentIds.Id).ToListAsync();
            return await ToDto(result);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Students
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
        }


        private async Task<List<StudentDto>> Todto(List<Student> students)
        {
            List<StudentDto> studentDtos = [];

            foreach (var student in students)
            {
                StudentDto studentDto = new StudentDto();
                studentDto.Id = student.Id;
                studentDto.Name = student.Name;
                studentDto.Email = student.Email;
                studentDto.PhoneNumber = student.PhoneNumber;
                studentDto.DateOfBirth = student.DateOfBirth;
                studentDtos.Add(studentDto);
            }
            return studentDtos;
        }
        private async Task<List<CourseStudentsDto>> ToDto(List<CourseStudent> CourseStudent)
        {
            List<CourseStudentsDto> CourseStudentDto = [];

            foreach (var course in CourseStudent)
            {
                CourseStudentsDto courseDto = new CourseStudentsDto();
                courseDto.CourseId = course.CourseId;
                courseDto.CourseName = course.Course.Name;
                CourseStudentDto.Add(courseDto);
            }
            return CourseStudentDto;
        }

        public async Task<bool> ExistsAsync(int? id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }
    }
}
