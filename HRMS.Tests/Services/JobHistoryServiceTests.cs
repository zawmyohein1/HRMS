using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Services;
using HRMS.Models.View;
using HRMS.Models.Entities;
using HRMS.DataAccess.Repositories.Interfaces;
using HRMS.Model.Responses;
using HRMS.Services.Validators.Interfaces;
using HRMS.Services.Cores.Implementations;
using HRMS.Models.Entities.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

public class JobHistoryServiceTests
{
    private readonly Mock<IJobHistoryRepository> _jobHistoryRepoMock;
    private readonly Mock<IEmployeeRepository> _employeeRepoMock;
    private readonly Mock<IJobRoleRepository> _jobRoleRepoMock;
    private readonly Mock<IJobHistoryValidator> _validatorMock;
    private readonly JobHistoryService _jobHistoryService;

    public JobHistoryServiceTests()
    {
        _jobHistoryRepoMock = new Mock<IJobHistoryRepository>();
        _employeeRepoMock = new Mock<IEmployeeRepository>();
        _jobRoleRepoMock = new Mock<IJobRoleRepository>();
        _validatorMock = new Mock<IJobHistoryValidator>();

        _jobHistoryService = new JobHistoryService(
            _jobHistoryRepoMock.Object,
            _employeeRepoMock.Object,
            _jobRoleRepoMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task GetsAsync_ShouldReturnJobHistories_WhenJobHistoriesExist()
    {
        // Arrange
        var jobHistories = new List<JobHistory>
        {
            new JobHistory { Id = 1, EmployeeId = 1, JobRoleId = 2, Status = "Active" }
        };

        _jobHistoryRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(jobHistories);

        // Act
        var result = await _jobHistoryService.GetsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnJobHistory_WhenJobHistoryExists()
    {
        // Arrange
        var jobHistory = new JobHistory { Id = 1, EmployeeId = 1, JobRoleId = 2 };

        _jobHistoryRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(jobHistory);
        _employeeRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(new List<Employee>());
        _jobRoleRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(new List<JobRole>());

        // Act
        var result = await _jobHistoryService.GetAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResponse_WhenValidJobHistory()
    {
        // Arrange
        var jobHistoryModel = new JobHistoryModel
        {
            EmployeeId = 1,
            JobRoleId = 2,
            StartDate = DateTime.Today.AddDays(-10),
            EndDate = DateTime.Today.AddDays(10),
            Status = "Active"
        };

        var jobHistoryEntity = new JobHistory
        {
            Id = 1,
            EmployeeId = 1,
            JobRoleId = 2,
            StartDate = jobHistoryModel.StartDate,
            EndDate = jobHistoryModel.EndDate,
            Status = jobHistoryModel.Status
        };

        _validatorMock.Setup(v => v.Validate(It.IsAny<JobHistoryModel>()))
                      .ReturnsAsync(new ResponseModel<JobHistoryModel>(200, "Validation Passed"));

        _jobHistoryRepoMock.Setup(repo => repo.AddAsync(It.IsAny<JobHistory>()))
                           .ReturnsAsync(jobHistoryEntity);

        _employeeRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(new List<Employee>());
        _jobRoleRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(new List<JobRole>());

        // Act
        var result = await _jobHistoryService.CreateAsync(jobHistoryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedResponse_WhenValidJobHistory()
    {
        // Arrange
        var jobHistoryModel = new JobHistoryModel { Id = 1, EmployeeId = 100, JobRoleId = 200 };
        var updatedEntity = new JobHistory { Id = 1, EmployeeId = 100, JobRoleId = 200 };

        _validatorMock.Setup(v => v.Validate(It.IsAny<JobHistoryModel>()))
                      .ReturnsAsync(new ResponseModel<JobHistoryModel>(200, "Validation Passed"));

        _jobHistoryRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<JobHistory>()))
                           .ReturnsAsync(updatedEntity);

        _employeeRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(new List<Employee>());
        _jobRoleRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(new List<JobRole>());

        // Act
        var result = await _jobHistoryService.UpdateAsync(jobHistoryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnDeletedResponse_WhenJobHistoryExists()
    {
        // Arrange
        var jobHistory = new JobHistory { Id = 1, EmployeeId = 1, JobRoleId = 2 };

        _jobHistoryRepoMock.Setup(repo => repo.DeleteAsync(1))
                           .ReturnsAsync(jobHistory);

        // Act
        var result = await _jobHistoryService.DeleteAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }
}
