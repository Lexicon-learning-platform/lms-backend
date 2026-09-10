namespace Lms_backend.Application.Models;

public class CourseWithActivitiesDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public int Duration { get; set; }
    public IEnumerable<ModuleDto> Modules { get; set; } = [];
}
