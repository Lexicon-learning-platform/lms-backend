namespace Lms_backend.Application.Models;

public class MyAssignmentsDto
{
    public IEnumerable<ActivitySimpleDto> Activities { get; set; } = [];
    public IEnumerable<TurninDto> Turnins { get; set; } = [];
}
