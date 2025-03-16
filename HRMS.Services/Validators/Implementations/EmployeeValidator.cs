using HRMS.Models.View;
using HRMS.DataAccess.Repositories.Interfaces;
using HRMS.Model.Responses;
using HRMS.Common.Messages;
using HRMS.Services.Validators.Interfaces;

namespace HRMS.Services.Validators.Implementations
{
    public class EmployeeValidator : IEmployeeValidator
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeValidator(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<ResponseModel<EmployeeModel>> Validate(EmployeeModel model)
        {
            if (model == null)
                return new ResponseModel<EmployeeModel>(400, string.Format(ResponseMessages.InvalidData, "employee"));

            // Ensure Employee Name is provided
            if (string.IsNullOrWhiteSpace(model.Name))
                return new ResponseModel<EmployeeModel>(400, string.Format(ResponseMessages.RequiredField, "Employee name"));

            // Validate Birth Date (Cannot be in the future)
            if (model.BirthDate > DateTime.Today)
                return new ResponseModel<EmployeeModel>(400, string.Format(ResponseMessages.InvalidDate, "Birth date"));

            // Validate Email (Check for duplicates if provided)
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var existingEmployee = await _employeeRepository.GetByEmailAsync(model.Email);
                if (existingEmployee != null && existingEmployee.Id != model.Id)
                    return new ResponseModel<EmployeeModel>(400, string.Format(ResponseMessages.DuplicateEntry, "Email"));
            }

            // Validate Phone Number (Check for duplicates if provided)
            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                var existingPhone = await _employeeRepository.GetByPhoneAsync(model.Phone);
                if (existingPhone != null && existingPhone.Id != model.Id)
                    return new ResponseModel<EmployeeModel>(400, string.Format(ResponseMessages.DuplicateEntry, "Phone number"));
            }

            return new ResponseModel<EmployeeModel>(200, ResponseMessages.Success);
        }

    }
}
