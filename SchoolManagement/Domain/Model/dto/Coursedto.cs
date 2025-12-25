
namespace SchoolManagement.dto
{
    public class CourseDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int TeacherId { get; set; }

        public List<string> StudentIds { get; set; } = [];
    }
}
