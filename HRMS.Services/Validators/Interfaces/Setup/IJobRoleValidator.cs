using HRMS.Model.Responses;
using System.Threading.Tasks;
using HRMS.Models.Views.Setup;

namespace HRMS.Services.Validators.Interfaces.Setup
{
    public interface IJobRoleValidator
    {
        Task<ResponseModel<JobRoleModel>> Validate(JobRoleModel model);
    }
}
