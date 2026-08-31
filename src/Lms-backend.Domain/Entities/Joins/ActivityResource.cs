using Lms_backend.Domain.Entities;
﻿namespace Lms_backend.Domain.Entities.Joins
{
    public record ActivityResource
    {
        public Guid Id { get; set; }

        public int ActivityId { get; set; }

        public int ResourceId { get; set; }

        public Activity? Activity { get; set; }

        public Resource? Resource { get; set; }
    }
}
