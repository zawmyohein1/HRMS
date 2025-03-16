using HRMS.Models.Entities;

namespace HRMS.DataAccess.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetByEmailAsync(string email);
        Task<Employee> GetByPhoneAsync(string phone);
    }
}
