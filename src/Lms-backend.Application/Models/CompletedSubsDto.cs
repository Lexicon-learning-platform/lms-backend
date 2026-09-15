using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Application.Models
{
    public class CompletedSubsDto
    {
        public List<string> CompletedSubs { get; set; } = new List<string>();

        public List<string> UnCompletedSubs { get; set; } = new List<string>();


    }
}
