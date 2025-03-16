using HRMS.Models.View;
using HRMS.Models.Entities;

public static class MapperJobHistory
{
    public static JobHistoryModel ToModel(JobHistory jobHistory, bool includeEmployee = true)
    {
        if (jobHistory == null) return null;

        return new JobHistoryModel
        {
            Id = jobHistory.Id,
            EmployeeId = jobHistory.EmployeeId,
            JobRoleId = jobHistory.JobRoleId,
            ManagerId = jobHistory.ManagerId,
            StartDate = jobHistory.StartDate,
            EndDate = jobHistory.EndDate,
            Status = jobHistory.Status ?? "Active",
            Comments = jobHistory.Comments ?? string.Empty,
            DepartmentId = jobHistory.JobRole?.DepartmentId ?? 0,
            EmployeeName = jobHistory.Employee?.Name,
            ManagerName = jobHistory.Manager?.Name,
            Title = jobHistory?.JobRole?.Title
        };
    }

    public static JobHistory ToEntity(JobHistoryModel model, bool includeEmployee = true)
    {
        if (model == null) return null;

        return new JobHistory
        {
            Id = model.Id,
            EmployeeId = model.EmployeeId,
            JobRoleId = model.JobRoleId,
            ManagerId = model.ManagerId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Status = model.Status,
            Comments = model.Comments,   
        };
    }

    public static List<JobHistoryModel> ToModeList(IEnumerable<JobHistory> jobHistories)
    {
        return jobHistories?.Select(jh => ToModel(jh, false)).ToList() ?? new List<JobHistoryModel>();
    }

    public static List<JobHistory> ToEntities(IEnumerable<JobHistoryModel> dtos)
    {
        return dtos?.Select(jh => ToEntity(jh, false)).ToList() ?? new List<JobHistory>();
    }
}