namespace SchoolManagement.Domain.Model
{
    public class Course
    {
        public int? Id { get; set; }             
        public string? Name { get; set; }

        public int TeacherId { get; set; }        
        public Teacher? Teacher { get; set; }

        public List<int> StudentIds { get; set; } = [];

        public List<CourseStudent> CourseStudents { get; set; } = [];
    }
}
