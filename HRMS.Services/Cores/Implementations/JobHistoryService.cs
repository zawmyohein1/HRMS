using System.Data;
using HRMS.Models.View;
using HRMS.Model.Responses;
using HRMS.DataAccess.Repositories.Interfaces;
using HRMS.Common.Messages;
using HRMS.Services.Validators.Interfaces;
using HRMS.Services.Cores.Interfaces;
using HRMS.DataAccess.Repositories.Interfaces.Setup;


namespace HRMS.Services.Cores.Implementations
{
    public class JobHistoryService : IJobHistoryService
    {
        private readonly IJobHistoryRepository _jobHistoryRepository;
        private readonly IJobRoleRepository _jobRoleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJobHistoryValidator _validator;

        public JobHistoryService(IJobHistoryRepository jobHistoryRepository, IEmployeeRepository employeeRepository, IJobRoleRepository jobRoleRepository, IJobHistoryValidator jobHistoryValidator)
        {
            _jobHistoryRepository = jobHistoryRepository;
            _employeeRepository = employeeRepository;
            _jobRoleRepository = jobRoleRepository;
            _validator = jobHistoryValidator;
        }

        public async Task<ResponseModel<IEnumerable<JobHistoryModel>>> GetsAsync()
        {
            try
            {
                var entities = await _jobHistoryRepository.GetsAsync();
                if (entities == null || !entities.Any())
                {
                    return new ResponseModel<IEnumerable<JobHistoryModel>>(404, string.Format(ResponseMessages.EntityListNotFound, "job histories"));
                }

                return new ResponseModel<IEnumerable<JobHistoryModel>>(200, ResponseMessages.Success, entities.Select(jh => MapperJobHistory.ToModel(jh)).ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetsAsync: {ex.Message}");
                return new ResponseModel<IEnumerable<JobHistoryModel>>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobHistoryModel>> GetAsync(int id)
        {
            try
            {
                var entity = await _jobHistoryRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<JobHistoryModel>(404, string.Format(ResponseMessages.EntityNotFound, "Job History", id));
                }


                var employees = await _employeeRepository.GetsAsync();
                var roles = await _jobRoleRepository.GetsAsync();

                var model = MapperJobHistory.ToModel(entity);
                model.Employees = MapperEmployee.ToModeList(employees);
                model.JobRoles = MapperJobRole.ToModeList(roles);


                return new ResponseModel<JobHistoryModel>(200, ResponseMessages.Success, model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return new ResponseModel<JobHistoryModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobHistoryModel>> GetJobHistoryAsync()
        {
            try
            {
                var employees = await _employeeRepository.GetsAsync();
                var roles = await _jobRoleRepository.GetsAsync();

                var model = new JobHistoryModel
                {
                    Employees = MapperEmployee.ToModeList(employees),
                    JobRoles = MapperJobRole.ToModeList(roles)
                };

                return new ResponseModel<JobHistoryModel>(200, ResponseMessages.Success, model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return new ResponseModel<JobHistoryModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobHistoryModel>> CreateAsync(JobHistoryModel model)
        {
            try
            {
                var validate = await _validator.Validate(model);
                if (validate.StatusCode != 200)
                {
                    var employees = await _employeeRepository.GetsAsync();
                    var roles = await _jobRoleRepository.GetsAsync();

                    validate.Data = model;
                    validate.Data.Employees = MapperEmployee.ToModeList(employees);
                    validate.Data.JobRoles = MapperJobRole.ToModeList(roles);
                    return validate; // Return validation error
                }

                var entity = MapperJobHistory.ToEntity(model);
                entity = await _jobHistoryRepository.AddAsync(entity);

                return new ResponseModel<JobHistoryModel>(201, string.Format(ResponseMessages.CreatedSuccessfully, "Job History"), MapperJobHistory.ToModel(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                return new ResponseModel<JobHistoryModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobHistoryModel>> UpdateAsync(JobHistoryModel model)
        {
            try
            {
                var validate = await _validator.Validate(model);
                if (validate.StatusCode != 200)
                {
                    var employees = await _employeeRepository.GetsAsync();
                    var roles = await _jobRoleRepository.GetsAsync();

                    validate.Data = model;
                    validate.Data.Employees = MapperEmployee.ToModeList(employees);
                    validate.Data.JobRoles = MapperJobRole.ToModeList(roles);

                    return validate;
                }

                var entity = MapperJobHistory.ToEntity(model);
                entity = await _jobHistoryRepository.UpdateAsync(entity);

                return new ResponseModel<JobHistoryModel>(200, string.Format(ResponseMessages.UpdatedSuccessfully, "Job History"), MapperJobHistory.ToModel(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                return new ResponseModel<JobHistoryModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<JobHistoryModel>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _jobHistoryRepository.DeleteAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<JobHistoryModel>(404, string.Format(ResponseMessages.EntityNotFound, "Job History", id));
                }

                return new ResponseModel<JobHistoryModel>(200, string.Format(ResponseMessages.DeletedSuccessfully, "Job History"), MapperJobHistory.ToModel(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                return new ResponseModel<JobHistoryModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }
    }
}

