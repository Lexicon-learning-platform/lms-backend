namespace Lms_backend.Application.Models;

public class UserSimpleDto
{
    public Guid Id { get; set; }
    public String GivenName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
