using System;
using System.Threading.Tasks;
using ASFS.Application.DTOs;
using ASFS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASFS.Api.Controllers
{
    [ApiController]
    [Route("api/approvals")]
    [Authorize(Roles = "Faculty,Admin")]
    public class ApprovalsController : ControllerBase
    {
        private readonly IApprovalService _approvalService;

        public ApprovalsController(IApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        [HttpGet("inbox")]
        public async Task<IActionResult> GetInbox()
        {
            var aadId = User.FindFirst("oid")?.Value;
            if (string.IsNullOrEmpty(aadId)) return Forbid();

            var inbox = await _approvalService.GetInboxAsync(aadId);
            return Ok(inbox);
        }

        [HttpPost("decide")]
        public async Task<IActionResult> Decide([FromBody] ApproveFormRequestDto dto)
        {
            var aadId = User.FindFirst("oid")?.Value;
            if (string.IsNullOrEmpty(aadId)) return Forbid();

            await _approvalService.ApproveAsync(aadId, dto);
            return NoContent();
        }
    }
}
