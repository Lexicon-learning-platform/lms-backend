namespace Lms_backend.Infrastructure.Models;

public class ModuleSearchParams(string? name, string? search, Guid? courseId) : SearchParams(name, search)
{
    public Guid? CourseId { get; init; } = courseId;
}
