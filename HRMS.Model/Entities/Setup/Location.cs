using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.Entities.Setup
{
    public class Location
    {
        public Location()
        {
            Departments = new List<Department>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Location name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.")]
        public string Country { get; set; } = string.Empty;

        public virtual ICollection<Department> Departments { get; set; }
    }
}
