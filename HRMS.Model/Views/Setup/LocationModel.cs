using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.Views.Setup
{
    public class LocationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Location name is required.")]
        [StringLength(100, ErrorMessage = "Location name must be a maximum of 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City name must be a maximum of 100 characters.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100, ErrorMessage = "Country name must be a maximum of 100 characters.")]
        public string Country { get; set; } = string.Empty;

        public ICollection<DepartmentModel> Departments { get; set; } = new List<DepartmentModel>();
    }
}
