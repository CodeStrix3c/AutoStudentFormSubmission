using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ASFS.Api.Controllers;
using ASFS.Application.DTOs;
using ASFS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ASFS.UnitTests
{
    public class FormsControllerTests
    {
        [Fact]
        public async Task Create_Should_Return_CreatedAt_With_Id()
        {
            // Arrange
            var svcMock = new Mock<IFormService>();
            svcMock.Setup(s => s.CreateAsync(It.IsAny<CreateFormRequestDto>(), It.IsAny<string>()))
                .ReturnsAsync(new FormResponseDto(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "aad-123",
                    "{}",
                    "Draft",
                    DateTimeOffset.UtcNow
                ));

            var controller = new FormsController(svcMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("oid", "aad-123")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var dto = new CreateFormRequestDto(Guid.NewGuid(), "{}");

            // Act
            var result = await controller.Create(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(FormsController.GetById), created.ActionName);
        }
    }
}
