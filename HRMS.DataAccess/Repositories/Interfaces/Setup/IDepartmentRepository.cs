using HRMS.Models.Entities.Setup;

namespace HRMS.DataAccess.Repositories.Interfaces.Setup
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department?> GetByNameAsync(string name);
        Task<bool> ExistsAsync(int id);
    }
}
