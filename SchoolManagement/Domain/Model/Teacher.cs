namespace SchoolManagement.Domain.Model
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

     
        public List<Course> CoursesTaught { get; set; } = new();
    }
}
