using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.Model;

namespace SchoolManagement.Service
{
    public class CourseService(SchoolDbContext context, ITeacherservice teacherService, IStudentService studentService) : ICourseService
    {
        public void Add(Course course)
        {
            if (course == null || course.Id != null)
            {
                return;
            }
            if (!teacherService.Exists(course.TeacherId))
            {
                return;
            }
            if (!DoStudentsExist(course))
            {
                return;
            }

            
            context.Courses.Add(course);
            context.SaveChanges();
        }

        public void Update(Course course)
        {
            if (course == null || course.Id == null)
            {
                return;
            }
            if (!Exists(course.Id))
            {
                return;
            }
            if (!teacherService.Exists(course.TeacherId))
            {
                return;
            }
            if (!DoStudentsExist(course))
            {
                return;
            }
            context.Courses.Update(course);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            context.Courses.Where(c => c.Id == Id).ExecuteDelete();
            context.SaveChanges();
        }

        private bool Exists(int? Id)
        {
            return context.Courses.Any(c => c.Id == Id);
        }

        private bool DoStudentsExist(Course course)
        {
            foreach(CourseStudent cs in course.CourseStudents)
            {
               int studentId = cs.StudentId;
                if (!studentService.Exists(studentId))
                {
                    return false;
                }
            }
            return true;
        }
    }

}
