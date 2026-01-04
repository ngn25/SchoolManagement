using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
using SchoolManagement.Domain.Model;
using SchoolManagement.Validation;

namespace SchoolManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly SchoolDbContext _context;

        public StudentService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AddStudentDto studentdto)
        {
            if (studentdto == null)
            {
                throw new ArgumentException("studentdto is null.");
            }
            ;


            if (!string.IsNullOrEmpty(studentdto.Email))
                Validator.ValidateEmail(studentdto.Email);

            if (!string.IsNullOrEmpty(studentdto.PhoneNumber))
                Validator.ValidatePhoneNumber(studentdto.PhoneNumber);

            Student student = studentdto.ToModel();

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

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

        public async Task DeleteAsync(int id)
        {
            await _context.Students
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsAsync(int? id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }
    }
}
