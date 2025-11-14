using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASFS.Domain.Entities;

namespace ASFS.Infrastructure.Repositories
{
    public interface IWorkflowRepository
    {
        Task<Workflow?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<ApprovalStep>> GetStepsAsync(Guid workflowId);
    }
}
