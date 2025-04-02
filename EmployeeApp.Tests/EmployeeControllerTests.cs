using EmployeeApp.Controllers;
using EmployeeApp.Dto.Employee.Request;
using EmployeeApp.Dto.Employee.Response;
using EmployeeApp.Serivce.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeApp.Tests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_employeeServiceMock.Object);
        }

        [Fact]
        public async Task AddEmployeeAsync_ShouldReturnOk_WhenEmployeeIsAdded()
        {
            // Arrange
            var request = new CreateEmployeeRequest { Name = "John", Surname = "Doe", Phone = "123456789" };
            _employeeServiceMock
                .Setup(service => service.AddEmployeeAsync(request))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.AddEmployeeAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, okResult.Value);
        }

        [Fact]
        public async Task AddEmployeeAsync_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Act
            var result = await _controller.AddEmployeeAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Employee cannot be null", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnNoContent_WhenEmployeeDeleted()
        {
            // Arrange
            _employeeServiceMock
                .Setup(service => service.DeleteEmployeeAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteEmployeeAsync(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnNotFound_WhenEmployeeNotDeleted()
        {
            // Arrange
            _employeeServiceMock
                .Setup(service => service.DeleteEmployeeAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteEmployeeAsync(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnOk_WhenEmployeeExists()
        {
            // Arrange
            var employee = new EmployeeResponse { Id = 1, Name = "John" };
            _employeeServiceMock
                .Setup(service => service.GetEmployeeByIdAsync(1))
                .ReturnsAsync(employee);

            // Act
            var result = await _controller.GetEmployeeByIdAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(employee, okResult.Value);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            _employeeServiceMock
                .Setup(service => service.GetEmployeeByIdAsync(1))
                .ReturnsAsync((EmployeeResponse)null);

            // Act
            var result = await _controller.GetEmployeeByIdAsync(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found", notFoundResult.Value);
        }
    }
}
