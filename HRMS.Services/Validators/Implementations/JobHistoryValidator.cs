using HRMS.Models.View;
using HRMS.DataAccess.Repositories.Interfaces;
using HRMS.Model.Responses;
using HRMS.Common.Messages;
using HRMS.Services.Validators.Interfaces;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

namespace HRMS.Services.Validators.Implementations
{
    public class JobHistoryValidator : IJobHistoryValidator
    {
        private readonly IJobHistoryRepository _jobHistoryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJobRoleRepository _jobRoleRepository;

        public JobHistoryValidator(IJobHistoryRepository jobHistoryRepository, IEmployeeRepository employeeRepository, IJobRoleRepository jobRoleRepository)
        {
            _jobHistoryRepository = jobHistoryRepository;
            _employeeRepository = employeeRepository;
            _jobRoleRepository = jobRoleRepository;
        }
        public async Task<ResponseModel<JobHistoryModel>> Validate(JobHistoryModel model)
        {
            if (model == null)
                return new ResponseModel<JobHistoryModel>(400, string.Format(ResponseMessages.InvalidData, "job history"));

            // Check if Employee Exists
            var employee = await _employeeRepository.GetByIdAsync(model.EmployeeId);
            if (employee == null)
                return new ResponseModel<JobHistoryModel>(400, string.Format(ResponseMessages.EntityNotFound, "Employee", model.EmployeeId));

            // Check if Job Role Exists
            var jobRole = await _jobRoleRepository.GetByIdAsync(model.JobRoleId);
            if (jobRole == null)
                return new ResponseModel<JobHistoryModel>(400, string.Format(ResponseMessages.EntityNotFound, "Job Role", model.JobRoleId));

            // Employee and Manager cannot be the same
            if (model.ManagerId.HasValue && model.ManagerId.Value == model.EmployeeId)
                return new ResponseModel<JobHistoryModel>(400, string.Format(ResponseMessages.InvalidData, "Employee and Manager cannot be the same"));

            // Validate Start Date (Cannot be in the future)
            if (model.StartDate > DateTime.Today)
                return new ResponseModel<JobHistoryModel>(400, string.Format(ResponseMessages.InvalidDate, "Start Date"));

            // Validate End Date (Must be later than Start Date if provided)
            if (model.EndDate.HasValue && model.EndDate < model.StartDate)
                return new ResponseModel<JobHistoryModel>(400, string.Format(ResponseMessages.InvalidDateRange, "End Date", "Start Date"));

            // Ensure Status is provided
            if (string.IsNullOrWhiteSpace(model.Status))
                return new ResponseModel<JobHistoryModel>(400, string.Format(ResponseMessages.RequiredField, "Status"));

            // Validate Manager (If ManagerId is provided)
            if (model.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(model.ManagerId.Value);
                if (manager == null)
                    return new ResponseModel<JobHistoryModel>(400, string.Format(ResponseMessages.EntityNotFound, "Manager", model.ManagerId.Value));
            }

            // Prevent duplicate job history records
            var existingJobHistory = await _jobHistoryRepository.GetByEmployeeAndManagerAsync(model.EmployeeId, model.ManagerId, model.Id);
            if (existingJobHistory != null)
                return new ResponseModel<JobHistoryModel>(400, string.Format(ResponseMessages.DuplicateEntry, "Job History"));

            return new ResponseModel<JobHistoryModel>(200, ResponseMessages.Success);
        }

    }
}
