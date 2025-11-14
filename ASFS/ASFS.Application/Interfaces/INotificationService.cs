using System.Threading.Tasks;

namespace ASFS.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyFormSubmittedAsync(string studentAadId, string formId);
        Task NotifyFormApprovedAsync(string studentAadId, string formId);
        Task NotifyFormRejectedAsync(string studentAadId, string formId, string? reason);
        Task NotifyApprovalAssignedAsync(string approverAadId, string formId, int stepOrder);
    }
}
