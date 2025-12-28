using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.Model;
using SchoolManagement.Validation;

namespace SchoolManagement.Service
{
    public class TeacherService : ITeacherservice
    {
        private readonly SchoolDbContext _context;

        public TeacherService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Teacher teacher)
        {
            if (teacher == null || teacher.Id != null)
            {
                throw new ArgumentException("Teacher is null or Id is not null.");
            }

            if (!string.IsNullOrEmpty(teacher.Email))
                Validator.ValidateEmail(teacher.Email);

            if (!string.IsNullOrEmpty(teacher.PhoneNumber))
                Validator.ValidatePhoneNumber(teacher.PhoneNumber);

            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            if (teacher == null || teacher.Id == null)
            {
                throw new ArgumentException("Teacher is null or Id is null.");
            }

            if (!await ExistsAsync(teacher.Id))
                return;

            if (!string.IsNullOrEmpty(teacher.Email))
                Validator.ValidateEmail(teacher.Email);

            if (!string.IsNullOrEmpty(teacher.PhoneNumber))
                Validator.ValidatePhoneNumber(teacher.PhoneNumber);


            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Teachers
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsAsync(int? id)
        {
            return await _context.Teachers.AnyAsync(t => t.Id == id);
        }
    }
}
