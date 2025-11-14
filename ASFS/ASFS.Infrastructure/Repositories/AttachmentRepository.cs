using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASFS.Domain.Entities;
using ASFS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ASFS.Infrastructure.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly ASFSDbContext _db;

        public AttachmentRepository(ASFSDbContext db)
        {
            _db = db;
        }

        public async Task<FormAttachment> AddAsync(FormAttachment attachment)
        {
            await _db.FormAttachments.AddAsync(attachment);
            await _db.SaveChangesAsync();
            return attachment;
        }

        public async Task<IReadOnlyList<FormAttachment>> GetByFormIdAsync(Guid formId)
        {
            return await _db.FormAttachments
                .Where(a => a.FormId == formId)
                .OrderByDescending(a => a.UploadedAt)
                .ToListAsync();
        }
    }
}
