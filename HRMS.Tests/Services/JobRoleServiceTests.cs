using Moq;
using HRMS.Model.Responses;
using HRMS.API.Controllers.Helper; // Ensure correct namespace for AutoMapper Profile
using AutoMapper;
using HRMS.Models.Entities.Setup;
using HRMS.Models.Views.Setup;
using HRMS.Services.Cores.Implementations.Setup;
using HRMS.Services.Validators.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

public class JobRoleServiceTests
{
    private readonly Mock<IJobRoleRepository> _jobRoleRepoMock;
    private readonly Mock<IJobRoleValidator> _validatorMock;
    private readonly JobRoleService _jobRoleService;
    private readonly Mock<IDepartmentRepository> _departmentRepoMock;

    public JobRoleServiceTests()
    {
        _jobRoleRepoMock = new Mock<IJobRoleRepository>();
        _validatorMock = new Mock<IJobRoleValidator>();
        _departmentRepoMock = new Mock<IDepartmentRepository>();

        // Configure AutoMapper for unit testing
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        var mapper = mapperConfig.CreateMapper();

        _jobRoleService = new JobRoleService(
            _jobRoleRepoMock.Object,
            mapper,
            _validatorMock.Object,
            _departmentRepoMock.Object
        );
    }

    [Fact]
    public async Task GetsAsync_ShouldReturnJobRoles_WhenJobRolesExist()
    {
        // Arrange
        var jobRoles = new List<JobRole>
        {
            new JobRole { Id = 1, Title = "Software Engineer", DepartmentId = 101 },
            new JobRole { Id = 2, Title = "Project Manager", DepartmentId = 102 }
        };

        _jobRoleRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(jobRoles);

        // Act
        var result = await _jobRoleService.GetsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetAsync_ShouldReturnJobRole_WhenJobRoleExists()
    {
        // Arrange
        var jobRole = new JobRole { Id = 1, Title = "Software Engineer", DepartmentId = 101 };

        _jobRoleRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(jobRole);

        // Act
        var result = await _jobRoleService.GetAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResponse_WhenValidJobRole()
    {
        // Arrange
        var jobRoleModel = new JobRoleModel { Title = "HR Manager", DepartmentId = 103 };
        var jobRoleEntity = new JobRole { Id = 1, Title = "HR Manager", DepartmentId = 103 };

        _validatorMock.Setup(v => v.Validate(It.IsAny<JobRoleModel>()))
                      .ReturnsAsync(new ResponseModel<JobRoleModel>(200, "Validation Passed"));

        _jobRoleRepoMock.Setup(repo => repo.AddAsync(It.IsAny<JobRole>()))
                        .ReturnsAsync(jobRoleEntity);

        // Act
        var result = await _jobRoleService.CreateAsync(jobRoleModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedResponse_WhenValidJobRole()
    {
        // Arrange
        var jobRoleModel = new JobRoleModel { Id = 1, Title = "Finance Manager", DepartmentId = 103 };
        var updatedEntity = new JobRole { Id = 1, Title = "Finance Manager", DepartmentId = 103 };

        _validatorMock.Setup(v => v.Validate(It.IsAny<JobRoleModel>()))
                      .ReturnsAsync(new ResponseModel<JobRoleModel>(200, "Validation Passed"));

        _jobRoleRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<JobRole>()))
                        .ReturnsAsync(updatedEntity);

        // Act
        var result = await _jobRoleService.UpdateAsync(jobRoleModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnDeletedResponse_WhenJobRoleExists()
    {
        // Arrange
        var jobRole = new JobRole { Id = 1, Title = "HR Specialist", DepartmentId = 101 };

        _jobRoleRepoMock.Setup(repo => repo.DeleteAsync(1))
                        .ReturnsAsync(jobRole);

        // Act
        var result = await _jobRoleService.DeleteAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }
}
