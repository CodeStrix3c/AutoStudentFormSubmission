using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASFS.Domain.Entities;
using ASFS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ASFS.Infrastructure.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly ASFSDbContext _db;

        public FormRepository(ASFSDbContext db)
        {
            _db = db;
        }

        public async Task<FormRequest> AddAsync(FormRequest form)
        {
            await _db.FormRequests.AddAsync(form);
            await _db.SaveChangesAsync();
            return form;
        }

        public async Task<FormRequest?> GetByIdAsync(Guid id)
        {
            return await _db.FormRequests
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task UpdateAsync(FormRequest form)
        {
            _db.FormRequests.Update(form);
            await _db.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<FormRequest>> GetByStudentAadAsync(string studentAadId)
        {
            return await _db.FormRequests
                .Where(f => f.StudentAadId == studentAadId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<FormRequest>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            return await _db.FormRequests
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
