using HRMS.Model.Responses;
using HRMS.Common.Messages;
using System.Threading.Tasks;
using HRMS.Models.Views.Setup;
using HRMS.Services.Validators.Interfaces.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

namespace HRMS.Services.Validators.Implementations.Setup
{
    public class JobRoleValidator : IJobRoleValidator
    {
        private readonly IJobRoleRepository _jobRoleRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public JobRoleValidator(IJobRoleRepository jobRoleRepository, IDepartmentRepository departmentRepository)
        {
            _jobRoleRepository = jobRoleRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<ResponseModel<JobRoleModel>> Validate(JobRoleModel model)
        {
            if (model == null)
                return new ResponseModel<JobRoleModel>(400, string.Format(ResponseMessages.InvalidData, "job role"));

            // Ensure Title is provided
            if (string.IsNullOrWhiteSpace(model.Title))
                return new ResponseModel<JobRoleModel>(400, string.Format(ResponseMessages.RequiredField, "Job title"));

            if (model.Title.Length > 100)
                return new ResponseModel<JobRoleModel>(400, string.Format(ResponseMessages.MaxLengthExceeded, "Job title", 100));

            // Ensure Description is valid (optional)
            if (!string.IsNullOrWhiteSpace(model.Description) && model.Description.Length > 250)
                return new ResponseModel<JobRoleModel>(400, string.Format(ResponseMessages.MaxLengthExceeded, "Description", 250));

            // Ensure Department exists
            var department = await _departmentRepository.GetByIdAsync(model.DepartmentId);
            if (department == null)
                return new ResponseModel<JobRoleModel>(400, string.Format(ResponseMessages.EntityNotFound, "Department", model.DepartmentId));

            // Prevent duplicate job role titles within the same department
            var existingJobRole = await _jobRoleRepository.GetByTitleAndDepartmentAsync(model.Title, model.DepartmentId);
            if (existingJobRole != null && existingJobRole.Id != model.Id)
            {
                return new ResponseModel<JobRoleModel>(400, string.Format(ResponseMessages.DuplicateEntry, "Job role"));
            }

            return new ResponseModel<JobRoleModel>(200, ResponseMessages.Success);
        }

    }
}
