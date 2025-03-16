using HRMS.Models.Entities;

namespace HRMS.DataAccess.Repositories.Interfaces
{
    public interface IJobHistoryRepository : IRepository<JobHistory>
    {
        Task<JobHistory> GetByEmployeeAndManagerAsync(int employeeId, int? managerId, int id);
    }
}
