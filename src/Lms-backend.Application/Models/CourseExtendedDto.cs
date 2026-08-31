namespace Lms_backend.Application.Models;

public class CourseExtendedDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public int Duration { get; set; }
    public ModuleDto[] Modules { get; set; } = [];
    public ResourceDto[] Resources { get; set; } = [];
}
