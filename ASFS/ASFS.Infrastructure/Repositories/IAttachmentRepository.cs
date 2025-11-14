using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASFS.Domain.Entities;

namespace ASFS.Infrastructure.Repositories
{
    public interface IAttachmentRepository
    {
        Task<FormAttachment> AddAsync(FormAttachment attachment);
        Task<IReadOnlyList<FormAttachment>> GetByFormIdAsync(Guid formId);
    }
}
