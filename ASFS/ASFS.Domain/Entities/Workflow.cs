using System;
using System.Collections.Generic;

namespace ASFS.Domain.Entities
{
    public class Workflow
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<ApprovalStep> Steps { get; set; } = new List<ApprovalStep>();
    }
}
