using HRMS.Model.Responses;
using HRMS.Models.Views.Setup;

namespace HRMS.Services.Validators.Interfaces.Setup
{
    public interface IDepartmentValidator
    {
        Task<ResponseModel<DepartmentModel>> Validate(DepartmentModel model);
    }
}
