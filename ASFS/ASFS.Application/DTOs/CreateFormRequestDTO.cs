using System;


namespace ASFS.Application.DTOs;


public record CreateFormRequestDto(Guid FormTypeId, string DataJson);