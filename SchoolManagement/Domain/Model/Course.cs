namespace SchoolManagement.Model
{
    public class Course
    {
        public int Id { get; set; }             
        public string Name { get; set; } = string.Empty;

        public int TeacherId { get; set; }        
        public Teacher? Teacher { get; set; }

        public ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();

        public override string ToString()
        {
            return $"{Id} | {Name} | Teacher: {Teacher?.Name ?? "None"} | Students: {CourseStudents.Count}";
        }
    }
}
