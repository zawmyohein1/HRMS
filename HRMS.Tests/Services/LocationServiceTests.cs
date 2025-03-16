using Moq;
using AutoMapper;
using HRMS.Model.Responses;
using HRMS.API.Controllers.Helper;
using HRMS.Models.Entities.Setup;
using HRMS.Models.Views.Setup;
using HRMS.Services.Cores.Implementations.Setup;
using HRMS.Services.Validators.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

public class LocationServiceTests
{
    private readonly Mock<ILocationRepository> _locationRepoMock;
    private readonly Mock<ILocationValidator> _validatorMock;
    private readonly LocationService _locationService;

    public LocationServiceTests()
    {
        _locationRepoMock = new Mock<ILocationRepository>();
        _validatorMock = new Mock<ILocationValidator>();

        // Configure AutoMapper for unit testing
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        var mapper = mapperConfig.CreateMapper();

        _locationService = new LocationService(
            _locationRepoMock.Object,
            mapper,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task GetsAsync_ShouldReturnLocations_WhenLocationsExist()
    {
        // Arrange
        var locations = new List<Location>
        {
            new Location { Id = 1, Name = "New York" },
            new Location { Id = 2, Name = "San Francisco" }
        };

        _locationRepoMock.Setup(repo => repo.GetsAsync()).ReturnsAsync(locations);

        // Act
        var result = await _locationService.GetsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetAsync_ShouldReturnLocation_WhenLocationExists()
    {
        // Arrange
        var location = new Location { Id = 1, Name = "New York" };

        _locationRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(location);

        // Act
        var result = await _locationService.GetAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnLocation_WhenLocationExists()
    {
        // Arrange
        var location = new Location { Id = 1, Name = "New York" };

        _locationRepoMock.Setup(repo => repo.GetByNameAsync("New York")).ReturnsAsync(location);

        // Act
        var result = await _locationService.GetByNameAsync("New York");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.Equal("New York", result.Data.Name);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResponse_WhenValidLocation()
    {
        // Arrange
        var locationModel = new LocationModel { Name = "Los Angeles" };
        var locationEntity = new Location { Id = 1, Name = "Los Angeles" };

        _validatorMock.Setup(v => v.Validate(It.IsAny<LocationModel>()))
                      .ReturnsAsync(new ResponseModel<LocationModel>(200, "Validation Passed"));

        _locationRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Location>()))
                         .ReturnsAsync(locationEntity);

        // Act
        var result = await _locationService.CreateAsync(locationModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedResponse_WhenValidLocation()
    {
        // Arrange
        var locationModel = new LocationModel { Id = 1, Name = "San Francisco Updated" };
        var updatedEntity = new Location { Id = 1, Name = "San Francisco Updated" };

        _validatorMock.Setup(v => v.Validate(It.IsAny<LocationModel>()))
                      .ReturnsAsync(new ResponseModel<LocationModel>(200, "Validation Passed"));

        _locationRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Location>()))
                         .ReturnsAsync(updatedEntity);

        // Act
        var result = await _locationService.UpdateAsync(locationModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnDeletedResponse_WhenLocationExists()
    {
        // Arrange
        var location = new Location { Id = 1, Name = "Miami" };

        _locationRepoMock.Setup(repo => repo.DeleteAsync(1))
                         .ReturnsAsync(location);

        // Act
        var result = await _locationService.DeleteAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(1, result.Data.Id);
    }
}
