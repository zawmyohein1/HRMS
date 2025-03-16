using AutoMapper;
using System.Data;
using HRMS.Model.Responses;
using HRMS.Common.Messages;
using HRMS.Models.Entities.Setup;
using HRMS.Models.Views.Setup;
using HRMS.Services.Cores.Interfaces.Setup;
using HRMS.Services.Validators.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

namespace HRMS.Services.Cores.Implementations.Setup
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly IDepartmentValidator _validator;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper, IDepartmentValidator validator, ILocationRepository locationRepository)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _validator = validator;
            _locationRepository = locationRepository;
        }

        public async Task<ResponseModel<IEnumerable<DepartmentModel>>> GetsAsync()
        {
            try
            {
                var entities = await _departmentRepository.GetsAsync();
                if (entities == null || !entities.Any())
                {
                    return new ResponseModel<IEnumerable<DepartmentModel>>(404, string.Format(ResponseMessages.EntityListNotFound, "departments"));
                }

                return new ResponseModel<IEnumerable<DepartmentModel>>(200, ResponseMessages.Success, _mapper.Map<IEnumerable<DepartmentModel>>(entities));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetsAsync: {ex.Message}");
                return new ResponseModel<IEnumerable<DepartmentModel>>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<DepartmentModel>> GetDepartmentAsync()
        {
            try
            {
                var locations = await _locationRepository.GetsAsync();

                var model = new DepartmentModel
                {
                    Locations = locations != null ? locations.Select(l => _mapper.Map<LocationModel>(l)).ToList() : new List<LocationModel>()

                };

                return new ResponseModel<DepartmentModel>(200, ResponseMessages.Success, _mapper.Map<DepartmentModel>(model));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return new ResponseModel<DepartmentModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<DepartmentModel>> GetAsync(int id)
        {
            try
            {
                var entity = await _departmentRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<DepartmentModel>(404, string.Format(ResponseMessages.EntityNotFound, "Department", id));
                }

                var model = _mapper.Map<DepartmentModel>(entity);
                var locations = await _locationRepository.GetsAsync();

                model.Locations = locations != null ? locations.Select(l => _mapper.Map<LocationModel>(l)).ToList() : new List<LocationModel>();

                return new ResponseModel<DepartmentModel>(200, ResponseMessages.Success, model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return new ResponseModel<DepartmentModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<DepartmentModel>> CreateAsync(DepartmentModel department)
        {
            try
            {
                var validate = await _validator.Validate(department);
                if (validate.StatusCode != 200)
                {
                    var locations = await _locationRepository.GetsAsync();
                    var locatonsModel = locations != null ? locations.Select(l => _mapper.Map<LocationModel>(l)).ToList() : new List<LocationModel>();

                    validate.Data = department;
                    validate.Data.Locations = locatonsModel;

                    return validate; // Return validation error
                }

                var entity = _mapper.Map<Department>(department);
                entity.Location = null; //force null to locations table
                entity = await _departmentRepository.AddAsync(entity);

                return new ResponseModel<DepartmentModel>(201, string.Format(ResponseMessages.CreatedSuccessfully, "Department"), _mapper.Map<DepartmentModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                return new ResponseModel<DepartmentModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<DepartmentModel>> UpdateAsync(DepartmentModel department)
        {
            try
            {
                var validate = await _validator.Validate(department);
                if (validate.StatusCode != 200)
                {
                    var locations = await _locationRepository.GetsAsync();
                    var locatonsModel = locations != null ? locations.Select(l => _mapper.Map<LocationModel>(l)).ToList() : new List<LocationModel>();

                    validate.Data = department;
                    validate.Data.Locations = locatonsModel;
                    return validate;
                }

                var entity = _mapper.Map<Department>(department);
                entity.Location = null; //force null to locations table
                entity = await _departmentRepository.UpdateAsync(entity);

                return new ResponseModel<DepartmentModel>(200, string.Format(ResponseMessages.UpdatedSuccessfully, "Department"), _mapper.Map<DepartmentModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                return new ResponseModel<DepartmentModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<DepartmentModel>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _departmentRepository.DeleteAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<DepartmentModel>(404, string.Format(ResponseMessages.EntityNotFound, "Department", id));
                }

                return new ResponseModel<DepartmentModel>(200, string.Format(ResponseMessages.DeletedSuccessfully, "Department"), _mapper.Map<DepartmentModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                return new ResponseModel<DepartmentModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }
    }
}

