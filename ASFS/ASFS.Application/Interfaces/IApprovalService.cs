using System.Collections.Generic;
using System.Threading.Tasks;
using ASFS.Application.DTOs;

namespace ASFS.Application.Interfaces
{
    public interface IApprovalService
    {
        Task<IReadOnlyList<ApprovalInboxItemDto>> GetInboxAsync(string approverAadId);
        Task ApproveAsync(string approverAadId, ApproveFormRequestDto dto);
    }
}
