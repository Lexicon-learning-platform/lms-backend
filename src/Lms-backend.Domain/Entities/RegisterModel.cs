using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Domain.Entities
{
    public class RegisterModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
