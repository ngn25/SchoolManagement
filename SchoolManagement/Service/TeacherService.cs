using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.dto;
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

            if (!string.IsNullOrEmpty(teacherdto.PhoneNumber))
                Validator.ValidatePhoneNumber(teacherdto.PhoneNumber);

            Teacher teacher = teacherdto.ToModel();
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
