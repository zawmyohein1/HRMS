using HRMS.Model.Responses;
using HRMS.Common.Messages;
using System.Threading.Tasks;
using HRMS.Models.Views.Setup;
using HRMS.Services.Validators.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

namespace HRMS.Services.Validators.Implementations.Setup
{
    public class DepartmentValidator : IDepartmentValidator
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILocationRepository _locationRepository;

        public DepartmentValidator(IDepartmentRepository departmentRepository, ILocationRepository locationRepository)
        {
            _departmentRepository = departmentRepository;
            _locationRepository = locationRepository;
        }

        public async Task<ResponseModel<DepartmentModel>> Validate(DepartmentModel model)
        {
            if (model == null)
                return new ResponseModel<DepartmentModel>(400, string.Format(ResponseMessages.InvalidData, "department"));

            // Ensure Name is provided
            if (string.IsNullOrWhiteSpace(model.Name))
                return new ResponseModel<DepartmentModel>(400, string.Format(ResponseMessages.RequiredField, "Department name"));

            if (model.Name.Length > 100)
                return new ResponseModel<DepartmentModel>(400, string.Format(ResponseMessages.MaxLengthExceeded, "Department name", 100));

            // Ensure Description is within limit (optional)
            if (!string.IsNullOrWhiteSpace(model.Description) && model.Description.Length > 250)
                return new ResponseModel<DepartmentModel>(400, string.Format(ResponseMessages.MaxLengthExceeded, "Description", 250));

            // Ensure Location exists
            var location = await _locationRepository.GetByIdAsync(model.LocationId);
            if (location == null)
                return new ResponseModel<DepartmentModel>(400, string.Format(ResponseMessages.EntityNotFound, "Location", model.LocationId));

            // Prevent duplicate department names
            var existingDepartment = await _departmentRepository.GetByNameAsync(model.Name);
            if (existingDepartment != null && existingDepartment.Id != model.Id)
                return new ResponseModel<DepartmentModel>(400, string.Format(ResponseMessages.DuplicateEntry, "department"));

            return new ResponseModel<DepartmentModel>(200, ResponseMessages.Success);
        }

    }
}