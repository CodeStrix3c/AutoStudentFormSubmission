using ASFS.Application.DTOs;
using ASFS.Application.Interfaces;
using ASFS.Domain.Entities;
using ASFS.Infrastructure.Repositories;

namespace ASFS.Application.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly IApprovalRepository _approvalRepo;
        private readonly IFormRepository _formRepo;

        public ApprovalService(IApprovalRepository approvalRepo, IFormRepository formRepo)
        {
            _approvalRepo = approvalRepo;
            _formRepo = formRepo;
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

            // naive logic: mark form as Approved/Rejected
            var form = await _formRepo.GetByIdAsync(approval.FormId);
            if (form == null) return;

            form.CurrentStatus = dto.Approve ? FormStatus.Approved : FormStatus.Rejected;
            form.UpdatedAt = DateTimeOffset.UtcNow;

            await _formRepo.UpdateAsync(form);
        }
    }
}
