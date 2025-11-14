using System;

namespace ASFS.Application.DTOs
{
    public record FormResponseDto(
        Guid Id,
        Guid FormTypeId,
        string? StudentAadId,
        string DataJson,
        string CurrentStatus,
        DateTimeOffset CreatedAt);
}
