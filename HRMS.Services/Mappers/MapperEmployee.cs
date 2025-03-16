using HRMS.Models.View;
using HRMS.Models.Entities;

public static class MapperEmployee
{
    public static EmployeeModel ToModel(Employee employee)
    {
        if (employee == null) return null;

        var model = new EmployeeModel
        {
            Id = employee.Id,
            Name = employee.Name,
            BirthDate = employee.BirthDate,
            Phone = employee.Phone,
            Email = employee.Email,
            Gender = employee.Gender,
            Address = employee.Address,
            Status = employee.Status,
            JobHistories = employee.JobHistories != null ? employee.JobHistories.Select(x => MapperJobHistory.ToModel(x)).ToList() : new List<JobHistoryModel>()
        };

        return model;

    }

    public static Employee ToEntity(EmployeeModel model, bool includeJobHistories = true)
    {
        if (model == null) return null;

        return new Employee
        {
            Id = model.Id,
            Name = model.Name,
            BirthDate = model.BirthDate,
            Phone = model.Phone,
            Email = model.Email,
            Gender = model.Gender,
            Address = model.Address,
            Status = model.Status
        };
    }

    public static List<EmployeeModel> ToModeList(IEnumerable<Employee> employees)
    {
        if (employees == null || !employees.Any()) return new List<EmployeeModel>();

        return employees.Select(e => ToModel(e)).ToList();
    }
}
