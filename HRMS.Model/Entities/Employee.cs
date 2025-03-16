using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.Entities
{
    public class Employee
    {
        public Employee()
        {
            JobHistories = new HashSet<JobHistory>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid phone number format.")]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        public virtual ICollection<JobHistory> JobHistories { get; set; }
    }
}
