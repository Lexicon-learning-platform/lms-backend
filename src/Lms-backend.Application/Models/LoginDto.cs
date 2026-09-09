using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Application.Models;

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

