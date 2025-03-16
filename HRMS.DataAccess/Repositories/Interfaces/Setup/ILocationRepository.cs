using HRMS.Models.Entities.Setup;

namespace HRMS.DataAccess.Repositories.Interfaces.Setup
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<Location> GetByNameAsync(string name);
    }
}
