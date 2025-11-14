using System;

namespace ASFS.Domain.Entities
{
    public enum ApprovalDecision
    {
        Pending,
        Approved,
        Rejected
    }

    public class Approval
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FormId { get; set; }
        public Guid? ApproverId { get; set; } // link to Users table if you add one
        public string? ApproverAadId { get; set; } // from Azure AD
        public int StepOrder { get; set; }
        public ApprovalDecision Decision { get; set; } = ApprovalDecision.Pending;
        public string? Comment { get; set; }
        public DateTimeOffset? DecisionAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
