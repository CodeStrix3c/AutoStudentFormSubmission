using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASFS.Application.DTOs;
using ASFS.Application.Interfaces;
using ASFS.Domain.Entities;
using ASFS.Infrastructure.Repositories;

namespace ASFS.Application.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _repo;
        private readonly INotificationService _notification;
        public FormService(IFormRepository repo, INotificationService notification)
        {
            _repo = repo;
            _notification = notification;
        }

        public async Task<FormResponseDto> CreateAsync(CreateFormRequestDto dto, string studentAadId)
        {
            var entity = new FormRequest
            {
                FormTypeId = dto.FormTypeId,
                StudentAadId = studentAadId,
                DataJson = dto.DataJson,
                CurrentStatus = FormStatus.Draft,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            var saved = await _repo.AddAsync(entity);

            return new FormResponseDto(
                saved.Id,
                saved.FormTypeId,
                saved.StudentAadId,
                saved.DataJson,
                saved.CurrentStatus.ToString(),
                saved.CreatedAt);
        }

        public async Task<FormResponseDto?> GetByIdAsync(Guid id)
        {
            var f = await _repo.GetByIdAsync(id);
            if (f == null) return null;

            return new FormResponseDto(
                f.Id,
                f.FormTypeId,
                f.StudentAadId,
                f.DataJson,
                f.CurrentStatus.ToString(),
                f.CreatedAt);
        }

        public async Task SubmitAsync(Guid id, string studentAadId)
        {
            var f = await _repo.GetByIdAsync(id);
            if (f == null)
                throw new InvalidOperationException("Form not found");

            if (!string.Equals(f.StudentAadId, studentAadId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Not the owner");

            if (f.CurrentStatus != FormStatus.Draft && f.CurrentStatus != FormStatus.Rejected)
                throw new InvalidOperationException("Only Draft/Rejected forms can be submitted");

            f.CurrentStatus = FormStatus.Submitted;
            f.SubmittedAt = DateTimeOffset.UtcNow;
            f.UpdatedAt = DateTimeOffset.UtcNow;

            await _repo.UpdateAsync(f);

            await _notification.NotifyFormSubmittedAsync(studentAadId, f.Id.ToString());
        }

        public async Task<IReadOnlyList<FormResponseDto>> GetMyFormsAsync(string studentAadId)
        {
            var forms = await _repo.GetByStudentAadAsync(studentAadId);
            return forms.Select(f =>
                new FormResponseDto(
                    f.Id,
                    f.FormTypeId,
                    f.StudentAadId,
                    f.DataJson,
                    f.CurrentStatus.ToString(),
                    f.CreatedAt
                )).ToList();
        }

        public async Task<IReadOnlyList<FormResponseDto>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            var forms = await _repo.GetAllAsync(page, pageSize);
            return forms.Select(f =>
                new FormResponseDto(
                    f.Id,
                    f.FormTypeId,
                    f.StudentAadId,
                    f.DataJson,
                    f.CurrentStatus.ToString(),
                    f.CreatedAt
                )).ToList();
        }
    }
}
