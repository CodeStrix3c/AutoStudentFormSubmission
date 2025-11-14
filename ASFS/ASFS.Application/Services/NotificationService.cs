using System.Threading.Tasks;
using ASFS.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ASFS.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IEmailSender emailSender,
        ISmsSender smsSender,
        ILogger<NotificationService> logger)
    {
        _emailSender = emailSender;
        _smsSender = smsSender;
        _logger = logger;
    }

    public Task NotifyFormSubmittedAsync(string studentAadId, string formId)
    {
        _logger.LogInformation("Notify Form Submitted: {FormId} for {Student}", formId, studentAadId);
        // Look up user email/phone using AadId via SIS/graph later.
        return Task.CompletedTask;
    }

    public Task NotifyFormApprovedAsync(string studentAadId, string formId)
    {
        _logger.LogInformation("Notify Form Approved: {FormId} for {Student}", formId, studentAadId);
        return Task.CompletedTask;
    }

    public Task NotifyFormRejectedAsync(string studentAadId, string formId, string? reason)
    {
        _logger.LogInformation("Notify Form Rejected: {FormId} for {Student}, Reason: {Reason}", formId, studentAadId, reason);
        return Task.CompletedTask;
    }

    public Task NotifyApprovalAssignedAsync(string approverAadId, string formId, int stepOrder)
    {
        _logger.LogInformation("Notify Approval Assigned: Form {FormId}, Step {Step}, Approver {Approver}",
            formId, stepOrder, approverAadId);
        return Task.CompletedTask;
    }
}
