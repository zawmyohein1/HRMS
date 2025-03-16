using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.Views.Setup
{
    public class DepartmentModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name must be a maximum of 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description must be a maximum of 250 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location ID is required.")]
        public int LocationId { get; set; }

        public string? LocationName { get; set; }

        public List<LocationModel>? Locations { get; set; } = new List<LocationModel>();
    }
}
