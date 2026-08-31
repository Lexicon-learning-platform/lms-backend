namespace Lms_backend.Application.Models;

public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public int Duration { get; set; }
    public ModuleSimpleDto[] Modules { get; set; } = [];
}
