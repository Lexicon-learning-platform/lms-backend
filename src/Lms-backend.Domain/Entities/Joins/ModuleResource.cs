using Lms_backend.Domain.Entities;
﻿namespace Lms_backend.Domain.Entities.Joins
{
    public record ModuleResource
    {
        public Guid Id { get; set; }

        public int ModuleId { get; set; }

        public int ResourceId { get; set; }

        public Module? Module { get; set; }

        public Resource? Resource { get; set; }
    }
}
