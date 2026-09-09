using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Application.Models;

    public class UserStatsDto
    {
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string[] Courses { get; set; } = [];
}

