using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASFS.Domain.Entities;

namespace ASFS.Infrastructure.Repositories
{
    public interface IApprovalRepository
    {
        Task<Approval> AddAsync(Approval approval);
        Task UpdateAsync(Approval approval);
        Task<Approval?> GetByIdAsync(Guid id);

        Task<IReadOnlyList<Approval>> GetInboxAsync(string approverAadId);
        Task<IReadOnlyList<Approval>> GetByFormIdAsync(Guid formId);
        Task<Approval?> GetPendingByFormAndStepAsync(Guid formId, int stepOrder);

    }
}
