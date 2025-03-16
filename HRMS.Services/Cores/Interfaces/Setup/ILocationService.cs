using HRMS.Model.Responses;
using HRMS.Models.Views.Setup;

namespace HRMS.Services.Cores.Interfaces.Setup
{
    public interface ILocationService
    {
        Task<ResponseModel<IEnumerable<LocationModel>>> GetsAsync();
        Task<ResponseModel<LocationModel>> GetAsync(int id);
        Task<ResponseModel<LocationModel>> CreateAsync(LocationModel location);
        Task<ResponseModel<LocationModel>> UpdateAsync(LocationModel location);
        Task<ResponseModel<LocationModel>> DeleteAsync(int id);
    }
}
