namespace SchoolManagement.Domain.Model
{
    public class Student
    {
        public int? Id { get; set; }
        public string? Name { get; set; } 
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
    }
}