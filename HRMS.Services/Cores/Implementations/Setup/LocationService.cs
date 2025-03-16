using AutoMapper;
using HRMS.Model.Responses;
using HRMS.Common.Messages;
using HRMS.Models.Entities.Setup;
using HRMS.Models.Views.Setup;
using HRMS.Services.Cores.Interfaces.Setup;
using HRMS.Services.Validators.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

namespace HRMS.Services.Cores.Implementations.Setup
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly ILocationValidator _validator;

        public LocationService(ILocationRepository locationRepository, IMapper mapper, ILocationValidator validator)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ResponseModel<IEnumerable<LocationModel>>> GetsAsync()
        {
            try
            {
                var entities = await _locationRepository.GetsAsync();
                if (entities == null || !entities.Any())
                {
                    return new ResponseModel<IEnumerable<LocationModel>>(404, string.Format(ResponseMessages.EntityListNotFound, "locations"));
                }

                return new ResponseModel<IEnumerable<LocationModel>>(200, ResponseMessages.Success, _mapper.Map<IEnumerable<LocationModel>>(entities));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetsAsync: {ex.Message}");
                return new ResponseModel<IEnumerable<LocationModel>>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<LocationModel>> GetAsync(int id)
        {
            try
            {
                var entity = await _locationRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<LocationModel>(404, string.Format(ResponseMessages.EntityNotFound, "Location", id));
                }

                return new ResponseModel<LocationModel>(200, ResponseMessages.Success, _mapper.Map<LocationModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return new ResponseModel<LocationModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<LocationModel>> GetByNameAsync(string name)
        {
            try
            {
                var entity = await _locationRepository.GetByNameAsync(name);
                if (entity == null)
                {
                    return new ResponseModel<LocationModel>(404, string.Format(ResponseMessages.EntityNotFound, "Location", name));
                }

                return new ResponseModel<LocationModel>(200, ResponseMessages.Success, _mapper.Map<LocationModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return new ResponseModel<LocationModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<LocationModel>> CreateAsync(LocationModel location)
        {
            try
            {
                var validate = await _validator.Validate(location);
                if (validate.StatusCode != 200)
                {
                    return validate; // Return validation error
                }

                var entity = _mapper.Map<Location>(location);
                entity.Departments = null;//force
                entity = await _locationRepository.AddAsync(entity);

                return new ResponseModel<LocationModel>(201, string.Format(ResponseMessages.CreatedSuccessfully, "Location"), _mapper.Map<LocationModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                return new ResponseModel<LocationModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<LocationModel>> UpdateAsync(LocationModel location)
        {
            try
            {
                var validate = await _validator.Validate(location);
                if (validate.StatusCode != 200)
                {
                    return validate; // Return validation error
                }

                var entity = _mapper.Map<Location>(location);
                entity.Departments = null;//force
                entity = await _locationRepository.UpdateAsync(entity);

                return new ResponseModel<LocationModel>(200, string.Format(ResponseMessages.UpdatedSuccessfully, "Location"), _mapper.Map<LocationModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                return new ResponseModel<LocationModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<LocationModel>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _locationRepository.DeleteAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<LocationModel>(404, string.Format(ResponseMessages.EntityNotFound, "Location", id));
                }

                return new ResponseModel<LocationModel>(200, string.Format(ResponseMessages.DeletedSuccessfully, "Location"), _mapper.Map<LocationModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                return new ResponseModel<LocationModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }
    }
}

