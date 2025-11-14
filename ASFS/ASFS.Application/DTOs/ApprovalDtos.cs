using System;

namespace ASFS.Application.DTOs
{
    public record ApprovalInboxItemDto(
        Guid ApprovalId,
        Guid FormId,
        int StepOrder,
        string FormStatus,
        DateTimeOffset CreatedAt);

    public record ApproveFormRequestDto(
        Guid ApprovalId,
        bool Approve,
        string? Comment);
}
