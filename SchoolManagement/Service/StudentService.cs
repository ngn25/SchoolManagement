using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
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

        public async Task AddAsync(Student student)
        {
            if (student == null || student.Id != null)
                return;

            if (!string.IsNullOrEmpty(student.Email))
                Validator.ValidateEmail(student.Email);

            if (!string.IsNullOrEmpty(student.PhoneNumber))
                Validator.ValidatePhoneNumber(student.PhoneNumber);


            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            if (student == null || student.Id == null)
                return;

            if (!await ExistsAsync(student.Id))
                return;
            if (!string.IsNullOrEmpty(student.Email))
                Validator.ValidateEmail(student.Email);

            if (!string.IsNullOrEmpty(student.PhoneNumber))
                Validator.ValidatePhoneNumber(student.PhoneNumber);



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
