namespace Lms_backend.Application.Models;

public class ModuleSimpleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Duration { get; set; }
}