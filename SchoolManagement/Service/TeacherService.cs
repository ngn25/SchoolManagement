using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public class TeacherService(SchoolDbContext context) : ITeacherservice
    {
        private readonly SchoolDbContext context = context;

        public void Add(Teacher teacher)
        {
            if (teacher == null || teacher.Id != null)
            {
                return;
            }

            context.Teachers.Add(teacher);
            context.SaveChanges();
        }

        public void Update(Student student)
        {
            if (student == null || student.Id == null)
            {
                return;
            }
            if (!Exists(student.Id))
            {
                return;
            }
            context.Students.Update(student);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            context.Students.Where(s => s.Id == Id).ExecuteDelete();
            context.SaveChanges();
        }

        private bool Exists(int? Id)
        {
            return context.Students.Any(s => s.Id == Id);
        }

        public void Add(Teacher teacher)
        {
            throw new NotImplementedException();
        }

        public void Update(Teacher teacher)
        {
            throw new NotImplementedException();
        }
    }
}
