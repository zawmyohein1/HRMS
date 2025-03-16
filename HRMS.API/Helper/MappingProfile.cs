using AutoMapper;
using HRMS.Models.Entities.Setup;
using HRMS.Models.Views.Setup;

namespace HRMS.API.Controllers.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DepartmentModel, Department>()
                .ForMember(dest => dest.Location, opt => opt.Ignore());

            CreateMap<Department, DepartmentModel>()
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : null));

            CreateMap<LocationModel, Location>();
            CreateMap<Location, LocationModel>();

            CreateMap<JobRoleModel, JobRole>()
              .ForMember(dest => dest.Department, opt => opt.Ignore());

            CreateMap<JobRole, JobRoleModel>()
                .ForMember(dest => dest.DepartmentName,
                           opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ReverseMap();
        }
    }
}