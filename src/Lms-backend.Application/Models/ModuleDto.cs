namespace Lms_backend.Application.Models;

public class ModuleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Duration { get; set; }
    public IEnumerable<ActivitySimpleDto> Activities { get; set; } = [];

}
