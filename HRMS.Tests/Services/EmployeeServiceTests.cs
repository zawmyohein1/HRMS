using Moq;
using HRMS.Models.View;
using HRMS.DataAccess.Repositories.Interfaces;
using HRMS.Model.Responses;
using HRMS.Models.Entities;
using HRMS.Services.Validators.Interfaces;
using HRMS.Services.Cores.Implementations;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepoMock;
    private readonly Mock<IDepartmentRepository> _departmentRepoMock;
    private readonly Mock<IEmployeeValidator> _validatorMock;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _employeeRepoMock = new Mock<IEmployeeRepository>();
        _departmentRepoMock = new Mock<IDepartmentRepository>();
        _validatorMock = new Mock<IEmployeeValidator>();

        _employeeService = new EmployeeService(
            _employeeRepoMock.Object,
            _departmentRepoMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResponse_WhenValidEmployee()
    {
        // Arrange
        var employeeModel = new EmployeeModel { Name = "John Doe", Gender = "Male", Status = "Active" };
        var employeeEntity = new Employee { Id = 1, Name = "John Doe" };

        _validatorMock.Setup(v => v.Validate(It.IsAny<EmployeeModel>()))
                      .ReturnsAsync(new ResponseModel<EmployeeModel>(200, "Validation Passed"));

        _employeeRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Employee>()))
                         .ReturnsAsync(employeeEntity);

        // Act
        var result = await _employeeService.CreateAsync(employeeModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal("John Doe", result.Data.Name);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnValidationError_WhenInvalidEmployee()
    {
        // Arrange
        var employeeModel = new EmployeeModel { Name = "", Gender = "Male", Status = "Active" }; // Invalid data

        _validatorMock.Setup(v => v.Validate(It.IsAny<EmployeeModel>()))
                      .ReturnsAsync(new ResponseModel<EmployeeModel>(400, "Invalid Data"));

        // Act
        var result = await _employeeService.CreateAsync(employeeModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Invalid Data", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedResponse_WhenValidEmployee()
    {
        // Arrange
        var employeeModel = new EmployeeModel { Id = 1, Name = "John Updated" };
        var updatedEntity = new Employee { Id = 1, Name = "John Updated" };

        _validatorMock.Setup(v => v.Validate(It.IsAny<EmployeeModel>()))
                      .ReturnsAsync(new ResponseModel<EmployeeModel>(200, "Validation Passed"));

        _employeeRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Employee>()))
                         .ReturnsAsync(updatedEntity);

        // Act
        var result = await _employeeService.UpdateAsync(employeeModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("John Updated", result.Data.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnDeletedResponse_WhenEmployeeExists()
    {
        // Arrange
        var employee = new Employee { Id = 1, Name = "John Doe" };

        _employeeRepoMock.Setup(repo => repo.DeleteAsync(1))
                         .ReturnsAsync(employee);

        // Act
        var result = await _employeeService.DeleteAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("John Doe", result.Data.Name);
    }
}
