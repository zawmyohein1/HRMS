using HRMS.Models.Entities.Setup;

namespace HRMS.DataAccess.Repositories.Interfaces.Setup
{
    public interface IJobRoleRepository : IRepository<JobRole>
    {
        Task<JobRole?> GetByTitleAndDepartmentAsync(string title, int departmentId);
        Task<JobRole?> GetByTitleAsync(string title);
    }
}