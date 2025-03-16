using AutoMapper;
using HRMS.DataAccess.Repositories.Interfaces;
using HRMS.Models.View;
using HRMS.Models.Requests;
using HRMS.Model.Responses;
using HRMS.Common.Messages;
using HRMS.Services.Validators.Interfaces;
using HRMS.Services.Cores.Interfaces;
using HRMS.Services.Mappers.Setup;
using HRMS.DataAccess.Repositories.Interfaces.Setup;

namespace HRMS.Services.Cores.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeValidator _validator;

        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRespository, IEmployeeValidator validator)
        {
            _departmentRepository = departmentRespository;
            _employeeRepository = employeeRepository;
            _validator = validator;
        }

        public async Task<ResponseModel<EmployeeDTOPage>> GetsAsync()
        {
            try
            {
                var entities = await _employeeRepository.GetsAsync();
                if (entities == null || !entities.Any())
                {
                    return new ResponseModel<EmployeeDTOPage>(404, string.Format(ResponseMessages.EntityListNotFound, "employees"));
                }

                var departments = await _departmentRepository.GetsAsync();
                var dtoDepartments = MapperDepartment.ToModeList(departments);

                var employeeDTO = MapperEmployee.ToModeList(entities);

                var employeeDTOPage = new EmployeeDTOPage()
                {
                    Employees = employeeDTO,
                    Departments = dtoDepartments
                };


                return new ResponseModel<EmployeeDTOPage>(200, ResponseMessages.Success, employeeDTOPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetsAsync: {ex.Message}");
                return new ResponseModel<EmployeeDTOPage>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<EmployeeModel>> GetAsync(int id)
        {
            try
            {
                var entity = await _employeeRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<EmployeeModel>(404, string.Format(ResponseMessages.EntityNotFound, "Employee", id));
                }

                return new ResponseModel<EmployeeModel>(200, ResponseMessages.Success, MapperEmployee.ToModel(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return new ResponseModel<EmployeeModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<EmployeeModel>> CreateAsync(EmployeeModel model)
        {
            try
            {
                var validationResult = await _validator.Validate(model);
                if (validationResult.StatusCode != 200)
                {
                    return validationResult; // Return validation error
                }

                var entity = MapperEmployee.ToEntity(model);
                entity = await _employeeRepository.AddAsync(entity);

                return new ResponseModel<EmployeeModel>(201, string.Format(ResponseMessages.CreatedSuccessfully, "Employee"), MapperEmployee.ToModel(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                return new ResponseModel<EmployeeModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<EmployeeModel>> UpdateAsync(EmployeeModel model)
        {
            try
            {
                var validationResult = await _validator.Validate(model);
                if (validationResult.StatusCode != 200)
                {
                    return validationResult;
                }

                var entity = MapperEmployee.ToEntity(model);
                entity = await _employeeRepository.UpdateAsync(entity);

                return new ResponseModel<EmployeeModel>(200, string.Format(ResponseMessages.UpdatedSuccessfully, "Employee"), MapperEmployee.ToModel(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                return new ResponseModel<EmployeeModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<EmployeeModel>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _employeeRepository.DeleteAsync(id);
                if (entity == null)
                {
                    return new ResponseModel<EmployeeModel>(404, string.Format(ResponseMessages.EntityNotFound, "Employee", id));
                }

                return new ResponseModel<EmployeeModel>(200, string.Format(ResponseMessages.DeletedSuccessfully, "Employee"), MapperEmployee.ToModel(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                return new ResponseModel<EmployeeModel>(500, ResponseMessages.InternalServerError, null, ex.Message);
            }
        }

        public async Task<ResponseModel<List<EmployeeModel>>> GetFilteredEmployeesAsync(EmployeeFilterRequest filterRequest)
        {
            var entities = await _employeeRepository.GetsAsync();
            if (entities == null || !entities.Any())
            {
                return new ResponseModel<List<EmployeeModel>>(404, string.Format(ResponseMessages.EntityListNotFound, "employees"));
            }

            var dtoDepartments = MapperEmployee.ToModeList(entities);

            var query = dtoDepartments.AsQueryable();

            if (filterRequest.EmployeeId > 0)
                query = query.Where(e => e.Id == filterRequest.EmployeeId);

            if (!string.IsNullOrEmpty(filterRequest.Name))
                query = query.Where(e => e.Name.Contains(filterRequest.Name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filterRequest.Gender))
                query = query.Where(e => e.Gender.Equals(filterRequest.Gender, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filterRequest.Status))
                query = query.Where(e => e.Status.Equals(filterRequest.Status, StringComparison.OrdinalIgnoreCase));

            if (filterRequest.DepartmentId.HasValue)
            {
                query = query.Where(e => e.JobHistories.Any(jh => jh.DepartmentId == filterRequest.DepartmentId.Value));
            }

            if (filterRequest.StartDate.HasValue)
            {
                query = query.Where(e => e.JobHistories.Any(jh => jh.StartDate >= filterRequest.StartDate.Value));
            }

            if (filterRequest.EndDate.HasValue)
            {
                query = query.Where(e => e.JobHistories.Any(jh => jh.EndDate.HasValue && jh.EndDate.Value <= filterRequest.EndDate.Value));
            }

            List<EmployeeModel> employees = query != null ? query.ToList() : [];

            return new ResponseModel<List<EmployeeModel>>(200, string.Format(ResponseMessages.Success, "Employee"), employees);
        }
    }
}
