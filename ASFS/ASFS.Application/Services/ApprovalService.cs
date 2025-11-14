using ASFS.Application.DTOs;
using ASFS.Application.Interfaces;
using ASFS.Domain.Entities;
using ASFS.Infrastructure.Repositories;

namespace ASFS.Application.Services;

public class ApprovalService : IApprovalService
{
    private readonly IApprovalRepository _approvalRepo;
    private readonly IFormRepository _formRepo;
    private readonly IWorkflowRepository _workflowRepo;
    private readonly INotificationService _notification;
    public ApprovalService(IApprovalRepository approvalRepo,
        IFormRepository formRepo,
        IWorkflowRepository workflowRepo,
        INotificationService notification)
    {
        _approvalRepo = approvalRepo;
        _formRepo = formRepo;
        _workflowRepo = workflowRepo;
        _notification = notification;
    }

    public async Task<IReadOnlyList<ApprovalInboxItemDto>> GetInboxAsync(string approverAadId)
    {
        var approvals = await _approvalRepo.GetInboxAsync(approverAadId);
        var result = new List<ApprovalInboxItemDto>();

        foreach (var a in approvals)
        {
            var form = await _formRepo.GetByIdAsync(a.FormId);
            if (form == null) continue;

            result.Add(new ApprovalInboxItemDto(
                a.Id,
                a.FormId,
                a.StepOrder,
                form.CurrentStatus.ToString(),
                a.CreatedAt
            ));
        }

        return result;
    }

    public async Task ApproveAsync(string approverAadId, ApproveFormRequestDto dto)
    {
        var approval = await _approvalRepo.GetByIdAsync(dto.ApprovalId);
        if (approval == null) throw new InvalidOperationException("Approval not found");

        if (!string.Equals(approval.ApproverAadId, approverAadId, StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Not your approval");

        if (approval.Decision != ApprovalDecision.Pending)
            throw new InvalidOperationException("Already decided");

        approval.Decision = dto.Approve ? ApprovalDecision.Approved : ApprovalDecision.Rejected;
        approval.Comment = dto.Comment;
        approval.DecisionAt = DateTimeOffset.UtcNow;

        await _approvalRepo.UpdateAsync(approval);

        var form = await _formRepo.GetByIdAsync(approval.FormId);
        if (form == null) return;

        if (!dto.Approve)
        {
            form.CurrentStatus = FormStatus.Rejected;
            form.UpdatedAt = DateTimeOffset.UtcNow;
            await _formRepo.UpdateAsync(form);

            if (!string.IsNullOrEmpty(form.StudentAadId))
                await _notification.NotifyFormRejectedAsync(form.StudentAadId, form.Id.ToString(), dto.Comment);

            return;
        }
        // APPROVED for this step -> check if more steps exist
        var workflow = await _workflowRepo.GetByIdAsync(form.WorkflowId);
        if (workflow == null || !workflow.Steps.Any())
        {
            // no workflow defined => move to Approved
            form.CurrentStatus = FormStatus.Approved;
            form.UpdatedAt = DateTimeOffset.UtcNow;
            await _formRepo.UpdateAsync(form);
            return;
        }

        var steps = workflow.Steps.OrderBy(s => s.StepOrder).ToList();
        var currentIndex = steps.FindIndex(s => s.StepOrder == approval.StepOrder);
        bool hasNext = currentIndex >= 0 && currentIndex < steps.Count - 1;

        if (!hasNext)
        {
            form.CurrentStatus = FormStatus.Approved;
            form.UpdatedAt = DateTimeOffset.UtcNow;
            await _formRepo.UpdateAsync(form);

            if (!string.IsNullOrEmpty(form.StudentAadId))
                await _notification.NotifyFormApprovedAsync(form.StudentAadId, form.Id.ToString());
        }
        else
        {
            // create next approval step
            var nextStep = steps[currentIndex + 1];

            var nextApproval = new Approval
            {
                FormId = form.Id,
                StepOrder = nextStep.StepOrder,
                // for now we don't know which exact user, only role/group
                // App can later assign an approver from that role
                ApproverAadId = null,
                Decision = ApprovalDecision.Pending,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await _approvalRepo.AddAsync(nextApproval);

            form.CurrentStatus = FormStatus.InReview;
            form.UpdatedAt = DateTimeOffset.UtcNow;
            await _formRepo.UpdateAsync(form);

            if (!string.IsNullOrEmpty(nextApproval.ApproverAadId))
            {
                await _notification.NotifyApprovalAssignedAsync(nextApproval.ApproverAadId, form.Id.ToString(), nextApproval.StepOrder);
            }
        }
    }
}
