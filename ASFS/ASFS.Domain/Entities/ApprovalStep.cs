using System;

namespace ASFS.Domain.Entities
{
    public class ApprovalStep
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; }
        public int StepOrder { get; set; }

        // Who approves at this step; later you can map to roles/departments
        public string ApproverRole { get; set; } = string.Empty; // e.g. "Faculty", "Dean"
        public string? ApproverAadGroupId { get; set; } // optional mapping to AAD group
    }
}
