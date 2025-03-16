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
    public class JobRoleService : IJobRoleService
    {
        private readonly IJobRoleRepository _jobRoleRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IJobRoleValidator _validator;

        public JobRoleService(IJobRoleRepository jobRoleRepository, IMapper mapper, IJobRoleValidator validator, IDepartmentRepository departmentRepository)
        {
            _jobRoleRepository = jobRoleRepository;
            _mapper = mapper;
            _validator = validator;
            _departmentRepository = departmentRepository;

        }

        public async Task<ResponseModel<IEnumerable<JobRoleModel>>> GetsAsync()
        {
            try
            {
                var entities = await _jobRoleRepository.GetsAsync();
                if (entities == null || !entities.Any())
                {
                    return new ResponseModel<IEnumerable<JobRoleModel>>(404, string.Format(ResponseMessages.EntityListNotFound, "job roles"));
                }

                return new ResponseModel<IEnumerable<JobRoleModel>>(200, ResponseMessages.Success, _mapper.Map<IEnumerable<JobRoleModel>>(entities));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetsAsync: {ex.Message}");
                return new ResponseModel<IEnumerable<JobRoleModel>>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobRoleModel>> GetAsync(int id)
        {
            try
            {
                var entity = await _jobRoleRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<JobRoleModel>(404, string.Format(ResponseMessages.EntityNotFound, "Job Role", id));
                }
                var deparments = await _departmentRepository.GetsAsync();

                var model = _mapper.Map<JobRoleModel>(entity);

                model.Departments = deparments != null ? deparments.Select(x => _mapper.Map<DepartmentModel>(x)).ToList() : new List<DepartmentModel>();

                return new ResponseModel<JobRoleModel>(200, ResponseMessages.Success, model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return new ResponseModel<JobRoleModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobRoleModel>> GetJobRoleAsync()
        {
            try
            {
                var deparments = await _departmentRepository.GetsAsync();

                var model = new JobRoleModel
                {
                    Departments = deparments != null ? deparments.Select(x => _mapper.Map<DepartmentModel>(x)).ToList() : new List<DepartmentModel>()
                };

                return new ResponseModel<JobRoleModel>(200, ResponseMessages.Success, model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return new ResponseModel<JobRoleModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobRoleModel>> CreateAsync(JobRoleModel jobRole)
        {
            try
            {
                var validate = await _validator.Validate(jobRole);
                if (validate.StatusCode != 200)
                {
                    var deparments = await _departmentRepository.GetsAsync();

                    validate.Data = jobRole;
                    validate.Data.Departments = deparments != null ?
                        deparments.Select(x => _mapper.Map<DepartmentModel>(x)).ToList() : new List<DepartmentModel>();

                    return validate; // Return validation error
                }

                var entity = _mapper.Map<JobRole>(jobRole);
                entity.Department = null;
                entity = await _jobRoleRepository.AddAsync(entity);

                return new ResponseModel<JobRoleModel>(201, string.Format(ResponseMessages.CreatedSuccessfully, "Job Role"), _mapper.Map<JobRoleModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                return new ResponseModel<JobRoleModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobRoleModel>> UpdateAsync(JobRoleModel jobRole)
        {
            try
            {
                var validate = await _validator.Validate(jobRole);
                if (validate.StatusCode != 200)
                {
                    var deparments = await _departmentRepository.GetsAsync();

                    validate.Data = jobRole;
                    validate.Data.Departments = deparments != null ?
                        deparments.Select(x => _mapper.Map<DepartmentModel>(x)).ToList() : new List<DepartmentModel>();

                    return validate; // Return validation error
                }

                var entity = _mapper.Map<JobRole>(jobRole);
                entity.Department = null;
                entity = await _jobRoleRepository.UpdateAsync(entity);

                return new ResponseModel<JobRoleModel>(200, string.Format(ResponseMessages.UpdatedSuccessfully, "Job Role"), _mapper.Map<JobRoleModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                return new ResponseModel<JobRoleModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobRoleModel>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _jobRoleRepository.DeleteAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<JobRoleModel>(404, string.Format(ResponseMessages.EntityNotFound, "Job Role", id));
                }

                return new ResponseModel<JobRoleModel>(200, string.Format(ResponseMessages.DeletedSuccessfully, "Job Role"), _mapper.Map<JobRoleModel>(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                return new ResponseModel<JobRoleModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }
    }

}