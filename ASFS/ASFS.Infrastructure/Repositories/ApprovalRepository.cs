using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASFS.Domain.Entities;
using ASFS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ASFS.Infrastructure.Repositories
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly ASFSDbContext _db;

        public ApprovalRepository(ASFSDbContext db)
        {
            _db = db;
        }

        public async Task<Approval> AddAsync(Approval approval)
        {
            await _db.Approvals.AddAsync(approval);
            await _db.SaveChangesAsync();
            return approval;
        }

        public async Task UpdateAsync(Approval approval)
        {
            _db.Approvals.Update(approval);
            await _db.SaveChangesAsync();
        }

        public async Task<Approval?> GetByIdAsync(Guid id)
        {
            return await _db.Approvals.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IReadOnlyList<Approval>> GetInboxAsync(string approverAadId)
        {
            return await _db.Approvals
                .Where(a => a.ApproverAadId == approverAadId && a.Decision == ApprovalDecision.Pending)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Approval>> GetByFormIdAsync(Guid formId)
        {
            return await _db.Approvals
                .Where(a => a.FormId == formId)
                .OrderBy(a => a.StepOrder)
                .ToListAsync();
        }
    }
}
