using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.Entities.Setup
{
    public class Department
    {
        public Department()
        {
            JobRoles = new List<JobRole>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; } = string.Empty;

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public virtual Location? Location { get; set; } = null!;

        public virtual ICollection<JobRole>? JobRoles { get; set; }
    }
}
