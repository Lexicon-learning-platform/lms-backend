namespace Lms_backend.Application.Models;

public class UserDto
{
    public Guid Id { get; set; }
    public String GivenName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
