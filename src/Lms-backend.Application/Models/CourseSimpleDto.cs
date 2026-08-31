namespace Lms_backend.Application.Models;

public class CourseSimpleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Duration { get; set; }
}
