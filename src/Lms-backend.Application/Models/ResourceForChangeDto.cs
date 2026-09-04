using System.ComponentModel.DataAnnotations;
using Lms_backend.Domain.Enums;

namespace Lms_backend.Application.Models;

public class ResourceForChangeDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public string Data { get; set; } = string.Empty;
}
