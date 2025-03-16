using HRMS.Model.Responses;
using HRMS.Models.Views.Setup;

namespace HRMS.Services.Cores.Interfaces.Setup
{
    public interface IJobRoleService
    {
        Task<ResponseModel<IEnumerable<JobRoleModel>>> GetsAsync();
        Task<ResponseModel<JobRoleModel>> GetAsync(int id);
        Task<ResponseModel<JobRoleModel>> GetJobRoleAsync();
        Task<ResponseModel<JobRoleModel>> CreateAsync(JobRoleModel jobRole);
        Task<ResponseModel<JobRoleModel>> UpdateAsync(JobRoleModel jobRole);
        Task<ResponseModel<JobRoleModel>> DeleteAsync(int id);
    }
}
