using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASFS.Domain.Entities;
using ASFS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ASFS.Infrastructure.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly ASFSDbContext _db;
        public WorkflowRepository(ASFSDbContext db) { _db = db; }

        public async Task<Workflow?> GetByIdAsync(Guid id)
        {
            return await _db.Workflows
                .Include(w => w.Steps)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IReadOnlyList<ApprovalStep>> GetStepsAsync(Guid workflowId)
        {
            return await _db.ApprovalSteps
                .Where(s => s.WorkflowId == workflowId)
                .OrderBy(s => s.StepOrder)
                .ToListAsync();
        }
    }
}
