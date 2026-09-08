using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Domain.Enums
{
    public enum ActionResponse
    {
        Success,
        Failure,
        NotFound,
        Unauthorized,
        BadRequest,
        UserAlreadyExists,
        UserNotFound,
        PasswordMismatch
    }
}
