using HRMS.Models.Entities.Setup;
using HRMS.Models.Views.Setup;

namespace HRMS.Services.Mappers.Setup
{
    public static class MapperDepartment
    {
        public static DepartmentModel ToModel(Department entity)
        {
            if (entity == null) return null;

            return new DepartmentModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                LocationId = entity.LocationId
            };
        }
        public static Department ToEntity(DepartmentModel model)
        {
            if (model == null) return null;

            return new Department
            {
                Id = model.Id, // Ensure Id is retained for updates
                Name = model.Name,
                Description = model.Description,
                LocationId = model.LocationId
            };
        }
        public static List<DepartmentModel> ToModeList(IEnumerable<Department> entities)
        {
            return entities?.Select(ToModel).ToList() ?? new List<DepartmentModel>();
        }

        public static List<Department> ToEntityList(IEnumerable<DepartmentModel> dtos)
        {
            return dtos?.Select(ToEntity).ToList() ?? new List<Department>();
        }

        internal static object ToModelList(object value)
        {
            throw new NotImplementedException();
        }
    }
}