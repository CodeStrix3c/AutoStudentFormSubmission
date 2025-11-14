using System;
using System.Threading.Tasks;
using ASFS.Application.DTOs;
using ASFS.Application.Services;
using ASFS.Domain.Entities;
using ASFS.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace ASFS.UnitTests
{
    public class FormServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_Save_Form_And_Return_Dto()
        {
            // Arrange
            var repoMock = new Mock<IFormRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<FormRequest>()))
                .ReturnsAsync((FormRequest f) => f);

            var service = new FormService(repoMock.Object);
            var dto = new CreateFormRequestDto(Guid.NewGuid(), "{\"field\":\"value\"}");
            var studentAad = "aad-123";

            // Act
            var result = await service.CreateAsync(dto, studentAad);

            // Assert
            Assert.Equal(dto.FormTypeId, result.FormTypeId);
            Assert.Equal(studentAad, result.StudentAadId);
            repoMock.Verify(r => r.AddAsync(It.IsAny<FormRequest>()), Times.Once);
        }
    }
}
