using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASFS.Domain.Entities;

namespace ASFS.Infrastructure.Repositories
{
    public interface IFormRepository
    {
        Task<FormRequest> AddAsync(FormRequest form);
        Task<FormRequest?> GetByIdAsync(Guid id);
        Task UpdateAsync(FormRequest form);

        Task<IReadOnlyList<FormRequest>> GetByStudentAadAsync(string studentAadId);
        Task<IReadOnlyList<FormRequest>> GetAllAsync(int page = 1, int pageSize = 50);
    }
}
