using EmployeeApp.Dto.Employee.Request;
using EmployeeApp.Entity;
using EmployeeApp.Repository.Interfaces;
using EmployeeApp.Serivce;
using Moq;

namespace EmployeeApp.Tests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_employeeRepositoryMock.Object);
        }

        [Fact]
        public async Task AddEmployeeAsync_ShouldReturnEmployeeId_WhenEmployeeIsAdded()
        {
            // Arrange
            var request = new CreateEmployeeRequest { Name = "John", Surname = "Doe", Phone = "123456789" };
            _employeeRepositoryMock
                .Setup(repo => repo.AddEmployeeAsync(It.IsAny<Employee>()))
                .ReturnsAsync(1);

            // Act
            var result = await _employeeService.AddEmployeeAsync(request);

            // Assert
            Assert.Equal(1, result);
            _employeeRepositoryMock.Verify(repo => repo.AddEmployeeAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task AddEmployeeAsync_ShouldThrowException_WhenRequestIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _employeeService.AddEmployeeAsync(null));
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenExists()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John", Surname = "Doe" };
            _employeeRepositoryMock
                .Setup(repo => repo.GetEmployeeByIdAsync(1))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.Name);
            _employeeRepositoryMock.Verify(repo => repo.GetEmployeeByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _employeeRepositoryMock
                .Setup(repo => repo.GetEmployeeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnTrue_WhenEmployeeDeleted()
        {
            // Arrange
            _employeeRepositoryMock
                .Setup(repo => repo.DeleteEmployeeAsync(1))
                .ReturnsAsync(1);

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnFalse_WhenEmployeeNotFound()
        {
            // Arrange
            _employeeRepositoryMock
                .Setup(repo => repo.DeleteEmployeeAsync(It.IsAny<int>()))
                .ReturnsAsync(0);

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(1);

            // Assert
            Assert.False(result);
        }
    }
}
