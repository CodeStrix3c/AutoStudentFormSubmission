using System;
using System.Threading.Tasks;
using ASFS.Application.DTOs;
using ASFS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASFS.Api.Controllers
{
    [ApiController]
    [Route("api/forms")]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormsController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateFormRequestDto dto)
        {
            var userAadId = User.FindFirst("oid")?.Value;
            if (string.IsNullOrEmpty(userAadId)) return Forbid();

            var res = await _formService.CreateAsync(dto, userAadId);
            return CreatedAtAction(nameof(GetById), new { id = res.Id }, res);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _formService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        // NEW: list forms for current student
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyForms()
        {
            var userAadId = User.FindFirst("oid")?.Value;
            if (string.IsNullOrEmpty(userAadId)) return Forbid();

            var list = await _formService.GetMyFormsAsync(userAadId);
            return Ok(list);
        }

        // NEW: list all forms (Admin/Faculty can see everything)
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var list = await _formService.GetAllAsync(page, pageSize);
            return Ok(list);
        }

        [HttpPost("{id:guid}/submit")]
        [Authorize]
        public async Task<IActionResult> Submit(Guid id)
        {
            var userAadId = User.FindFirst("oid")?.Value;
            if (string.IsNullOrEmpty(userAadId)) return Forbid();

            await _formService.SubmitAsync(id, userAadId);
            return NoContent();
        }
    }
}
