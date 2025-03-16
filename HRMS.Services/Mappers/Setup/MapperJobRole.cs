using HRMS.Models.Entities.Setup;
using HRMS.Models.Views.Setup;

public static class MapperJobRole
{
    public static JobRoleModel ToModel(JobRole jobRole)
    {
        try
        {
            if (jobRole == null)
                return null;

            return new JobRoleModel
            {
                Id = jobRole.Id,
                Title = jobRole.Title,
                Description = jobRole.Description,
                DepartmentId = jobRole.DepartmentId
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public static List<JobRoleModel> ToModeList(IEnumerable<JobRole> jobRoles)
    {
        try
        {
            return jobRoles?.Select(ToModel).ToList() ?? new List<JobRoleModel>();
        }
        catch (Exception ex)
        {
            return new List<JobRoleModel>();
        }
    }
    public static JobRole ToEntity(JobRoleModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return new JobRole
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DepartmentId = model.DepartmentId
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}