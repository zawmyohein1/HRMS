using HRMS.Model.Responses;
using HRMS.Common.Messages;
using HRMS.Models.Views.Setup;
using HRMS.Services.Validators.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

namespace HRMS.Services.Validators.Implementations.Setup
{
    public class LocationValidator : ILocationValidator
    {
        private readonly ILocationRepository _locationRepository;

        public LocationValidator(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<ResponseModel<LocationModel>> Validate(LocationModel model)
        {
            if (model == null)
                return new ResponseModel<LocationModel>(400, string.Format(ResponseMessages.InvalidData, "location"));

            // Ensure Location Name is provided
            if (string.IsNullOrWhiteSpace(model.Name))
                return new ResponseModel<LocationModel>(400, string.Format(ResponseMessages.RequiredField, "Location name"));

            // Prevent duplicate locations
            var existingLocation = await _locationRepository.GetByNameAsync(model.Name);
            if (existingLocation != null && existingLocation.Id != model.Id)
            {
                return new ResponseModel<LocationModel>(400, string.Format(ResponseMessages.DuplicateEntry, "Location"));
            }

            return new ResponseModel<LocationModel>(200, ResponseMessages.Success);
        }

    }
}
