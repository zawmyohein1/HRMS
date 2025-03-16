using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.Entities.Setup
{
    public class JobRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Job title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; } = string.Empty;

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        public virtual Department? Department { get; set; } = null!;
    }
}
