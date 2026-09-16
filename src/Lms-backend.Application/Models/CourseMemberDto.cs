namespace Lms_backend.Application.Models;

public class CourseMemberDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? GivenName { get; set; }
    public string? LastName { get; set; }
    public string? Role { get; set; }
}