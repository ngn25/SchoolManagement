using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public class TeacherService(SchoolDbContext context) : ITeacherservice
    {
        public void Add(Teacher teacher)
        {
            if (teacher == null || teacher.Id != null)
            {
                return;
            }

            context.Teachers.Add(teacher);
            context.SaveChanges();
        }

        public void Update(Teacher teacher)
        {
            if (teacher == null || teacher.Id == null)
            {
                return;
            }
            if (!Exists(teacher.Id))
            {
                return;
            }
            context.Teachers.Update(teacher);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            context.Teachers.Where(t => t.Id == Id).ExecuteDelete();
            context.SaveChanges();
        }

        public bool Exists(int? Id)
        {
            return context.Teachers.Any(t => t.Id == Id);
        }
    }
}
