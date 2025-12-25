using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public class StudentService(SchoolDbContext context) : IStudentService
    {
        public void Add(Student student)
        {
            if (student == null || student.Id != null)
            {
                return;
            }

            context.Students.Add(student);
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

        public bool Exists(int? Id)
        {
            return context.Students.Any(s => s.Id == Id);
        }
    }
}
