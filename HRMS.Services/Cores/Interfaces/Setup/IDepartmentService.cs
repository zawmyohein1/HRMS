using HRMS.Model.Responses;
using HRMS.Models.Views.Setup;

namespace HRMS.Services.Cores.Interfaces.Setup
{
    public interface IDepartmentService
    {
        Task<ResponseModel<IEnumerable<DepartmentModel>>> GetsAsync();
        Task<ResponseModel<DepartmentModel>> GetAsync(int id);
        Task<ResponseModel<DepartmentModel>> GetDepartmentAsync();
        Task<ResponseModel<DepartmentModel>> CreateAsync(DepartmentModel department);
        Task<ResponseModel<DepartmentModel>> UpdateAsync(DepartmentModel department);
        Task<ResponseModel<DepartmentModel>> DeleteAsync(int id);
    }
}
