using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.Entities.Setup;

namespace HRMS.Models.Entities
{
    public class JobHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //  Required Foreign Key for Employee
        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        //  Required Foreign Key for JobRole
        [Required]
        [ForeignKey("JobRole")]
        public int JobRoleId { get; set; }
        public JobRole? JobRole { get; set; }

        //  Nullable Foreign Key for Manager
        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; } // Self-referencing FK to Employee

        //  Required StartDate with Default Value
        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        //  Nullable EndDate (Employee may still be active)
        public DateTime? EndDate { get; set; }

        //  Required Status with Default Value
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Consider using an Enum

        //  Optional Comments with Length Constraint
        [StringLength(500)]
        public string? Comments { get; set; }
    }
}
