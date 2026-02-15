
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public class CourseStudentService : ICourseStudentService
    {
        private readonly SchoolDbContext _context;

        public CourseStudentService(SchoolDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(CourseStudentDto dto)
        {
            if (dto == null)
                throw new ArgumentException("CourseStudentDto is null.");


            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == dto.StudentId);
            if (student == null)
                throw new KeyNotFoundException($"Student with Id {dto.StudentId} not found.");


            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == dto.CourseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with Id {dto.CourseId} not found.");

            var existing = await _context.CourseStudents
                .FirstOrDefaultAsync(cs => cs.StudentId == dto.StudentId && cs.CourseId == dto.CourseId);

            if (existing != null)
                throw new InvalidOperationException("Student is already registered in this course.");

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

            if (entity != null)
            {
                _context.CourseStudents.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CourseSimpleDto>> GetCoursesByStudentAsync(int studentId)
        {
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
            var list = await _context.CourseStudents
                .OrderBy(p => p.CourseId).ThenBy(p => p.StudentId).ToListAsync();

            return await ToDto(list);
        }


        private async Task<List<CourseStudentDto>> ToDto(List<CourseStudent> entities)
        {
            List<CourseStudentDto> dtos = new List<CourseStudentDto>();

            foreach (var cs in entities)
            {
                var dto = new CourseStudentDto
                {
                    StudentId = cs.StudentId,
                    CourseId = cs.CourseId
                };
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
