using HRMS.Model.Responses;
using HRMS.Models.View;

namespace HRMS.Services.Cores.Interfaces
{
    public interface IJobHistoryService
    {
        Task<ResponseModel<IEnumerable<JobHistoryModel>>> GetsAsync();
        Task<ResponseModel<JobHistoryModel>> GetAsync(int id);
        Task<ResponseModel<JobHistoryModel>> GetJobHistoryAsync();
        Task<ResponseModel<JobHistoryModel>> CreateAsync(JobHistoryModel jobHistory);
        Task<ResponseModel<JobHistoryModel>> UpdateAsync(JobHistoryModel jobHistory);
        Task<ResponseModel<JobHistoryModel>> DeleteAsync(int id);
    }
}
