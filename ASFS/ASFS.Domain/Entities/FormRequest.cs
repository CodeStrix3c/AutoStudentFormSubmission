using System;


namespace ASFS.Domain.Entities;


public enum FormStatus { Draft, Submitted, InReview, Approved, Rejected, Withdrawn }


public class FormRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FormTypeId { get; set; }
    public Guid? StudentId { get; set; }
    public string? StudentAadId { get; set; }
    public string DataJson { get; set; } = string.Empty;
    public FormStatus CurrentStatus { get; set; } = FormStatus.Draft;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? SubmittedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid WorkflowId { get; set; }   

}