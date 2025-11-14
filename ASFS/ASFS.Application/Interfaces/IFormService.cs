using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASFS.Application.DTOs;

namespace ASFS.Application.Interfaces
{
    public interface IFormService
    {
        Task<FormResponseDto> CreateAsync(CreateFormRequestDto dto, string studentAadId);
        Task<FormResponseDto?> GetByIdAsync(Guid id);
        Task SubmitAsync(Guid id, string studentAadId);

        Task<IReadOnlyList<FormResponseDto>> GetMyFormsAsync(string studentAadId);
        Task<IReadOnlyList<FormResponseDto>> GetAllAsync(int page = 1, int pageSize = 50);
    }
}
