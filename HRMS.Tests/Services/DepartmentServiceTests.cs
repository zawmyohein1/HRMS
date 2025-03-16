using Moq;
using AutoMapper;
using HRMS.Model.Responses;
using HRMS.API.Controllers.Helper;
using HRMS.Models.Entities.Setup;
using HRMS.Models.Views.Setup;
using HRMS.Services.Cores.Implementations.Setup;
using HRMS.Services.Validators.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepoMock;
    private readonly Mock<IDepartmentValidator> _validatorMock;
    private readonly DepartmentService _departmentService;
    private readonly Mock<ILocationRepository> _locatoinsRepoMock;

    public DepartmentServiceTests()
    {
        _departmentRepoMock = new Mock<IDepartmentRepository>();
        _validatorMock = new Mock<IDepartmentValidator>();
        _locatoinsRepoMock = new Mock<ILocationRepository>();

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        var mapper = mapperConfig.CreateMapper();

        _departmentService = new DepartmentService(
            _departmentRepoMock.Object,
            mapper,
            _validatorMock.Object,
            _locatoinsRepoMock.Object
        );
    }

    [Fact]
    public async Task GetsAsync_ShouldReturnDepartments_WhenDepartmentsExist()
    {
        // Arrange
        var departments = new List<Department>
        {
            new Department { Id = 1, Name = "HR", LocationId = 101 },
            new Department { Id = 2, Name = "IT", LocationId = 102 }
        };

        _departmentRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(departments);

        // Act
        var result = await _departmentService.GetsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDepartment_WhenDepartmentExists()
    {
        // Arrange
        var department = new Department { Id = 1, Name = "HR", LocationId = 101 };

        _departmentRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(department);

        // Act
        var result = await _departmentService.GetAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResponse_WhenValidDepartment()
    {
        // Arrange
        var departmentModel = new DepartmentModel { Name = "Finance", LocationId = 103 };
        var departmentEntity = new Department { Id = 1, Name = "Finance", LocationId = 103 };

        _validatorMock.Setup(v => v.Validate(It.IsAny<DepartmentModel>()))
                      .ReturnsAsync(new ResponseModel<DepartmentModel>(200, "Validation Passed"));

        _departmentRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Department>()))
                           .ReturnsAsync(departmentEntity);

        // Act
        var result = await _departmentService.CreateAsync(departmentModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedResponse_WhenValidDepartment()
    {
        // Arrange
        var departmentModel = new DepartmentModel { Id = 1, Name = "Finance Updated", LocationId = 103 };
        var updatedEntity = new Department { Id = 1, Name = "Finance Updated", LocationId = 103 };

        _validatorMock.Setup(v => v.Validate(It.IsAny<DepartmentModel>()))
                      .ReturnsAsync(new ResponseModel<DepartmentModel>(200, "Validation Passed"));

        _departmentRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Department>()))
                           .ReturnsAsync(updatedEntity);

        // Act
        var result = await _departmentService.UpdateAsync(departmentModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnDeletedResponse_WhenDepartmentExists()
    {
        // Arrange
        var department = new Department { Id = 1, Name = "HR", LocationId = 101 };

        _departmentRepoMock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(department);

        // Act
        var result = await _departmentService.DeleteAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }
}
