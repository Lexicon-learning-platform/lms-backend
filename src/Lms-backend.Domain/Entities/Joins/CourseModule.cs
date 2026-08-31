using Lms_backend.Domain.Entities;
﻿namespace Lms_backend.Domain.Entities.Joins
{
    public record CourseModule
    {
        public Guid Id { get; set; }

        public int CourseId { get; set; }

        public int ModuleId { get; set; }

        public Course? Course { get; set; }

        public Module? Module { get; set; }
    }
}
