using HRMS.Models.View;
using HRMS.Models.Requests;
using HRMS.Model.Responses;

namespace HRMS.Services.Cores.Interfaces
{
    public interface IEmployeeService
    {
        Task<ResponseModel<EmployeeDTOPage>> GetsAsync();
        Task<ResponseModel<EmployeeModel>> GetAsync(int id);
        Task<ResponseModel<EmployeeModel>> CreateAsync(EmployeeModel employee);
        Task<ResponseModel<EmployeeModel>> UpdateAsync(EmployeeModel employee);
        Task<ResponseModel<EmployeeModel>> DeleteAsync(int id);
        Task<ResponseModel<List<EmployeeModel>>> GetFilteredEmployeesAsync(EmployeeFilterRequest filterRequest);
    }
}