using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.Views.Setup
{
    public class JobRoleModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(100, ErrorMessage = "Job title must be a maximum of 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description must be a maximum of 250 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department ID is required.")]
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public List<DepartmentModel>? Departments { get; set; }
    }
}
