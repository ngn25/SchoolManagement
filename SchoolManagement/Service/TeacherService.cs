using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using SchoolManagement.Validation;
using Microsoft.AspNetCore.Authorization;


namespace SchoolManagement.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly SchoolDbContext _context;

        public TeacherService(SchoolDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task AddAsync(AddTeacherDto teacherdto)
        {
            if (teacherdto == null)
            {
                throw new ArgumentException("teacherdto is null.");
            }

            if (!string.IsNullOrEmpty(teacherdto.Email))
                Validator.ValidateEmail(teacherdto.Email);

            if (!string.IsNullOrEmpty(teacherdto.PhoneNumber))
                Validator.ValidatePhoneNumber(teacherdto.PhoneNumber);

            Teacher teacher = teacherdto.ToModel();

            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        [Authorize]
        public async Task UpdateAsync(TeacherDto teacherdto)
        {
            if (teacherdto == null)
            {
                throw new ArgumentException("teacherdto is null .");
            }

            if (!await ExistsAsync(teacherdto.Id))
                return;

            if (!string.IsNullOrEmpty(teacherdto.Email))
                Validator.ValidateEmail(teacherdto.Email);

            if (_context.Teachers.Any(p => p.Email == teacherdto.Email))
            {
                throw new ArgumentException("already exist .");
            }

            if (!string.IsNullOrEmpty(teacherdto.PhoneNumber))
                Validator.ValidatePhoneNumber(teacherdto.PhoneNumber);
            if (_context.Teachers.Any(p => p.PhoneNumber == teacherdto.PhoneNumber))
            {
                throw new ArgumentException("already exist .");
            }


            Teacher teacher = teacherdto.ToModel();
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }
        [Authorize]
        public async Task<List<CourseDto>> GetCoursesByEmail(string Email)
        {

            if (!string.IsNullOrEmpty(Email))
                Validator.ValidateEmail(Email);

            var result = await _context.Courses.Where(p => p.Teacher.Email == Email).ToListAsync();

            return await ToDto(result);
        }

        [Authorize]
        public async Task<List<TeacherDto>> GetAll()
        {
            var a = await _context.Teachers.ToListAsync();

            return await Todto(a);
        }
        [Authorize]
        public async Task DeleteAsync(int id)
        {
            await _context.Teachers
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();
        }
        private async Task<List<TeacherDto>> Todto(List<Teacher> teachers)
        {
            List<TeacherDto> teacherDtos = [];

            foreach (var teacher in teachers)
            {
                TeacherDto teacherDto = new TeacherDto();
                teacherDto.Id = teacher.Id;
                teacherDto.Name = teacher.Name;
                teacherDto.Email = teacher.Email;
                teacherDto.PhoneNumber = teacher.PhoneNumber;
                teacherDtos.Add(teacherDto);
            }
            return teacherDtos;
        }


        private async Task<List<CourseDto>> ToDto(List<Course> courses)
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

        public async Task<bool> ExistsAsync(int? id)
        {
            return await _context.Teachers.AnyAsync(t => t.Id == id);
        }
    }
}
