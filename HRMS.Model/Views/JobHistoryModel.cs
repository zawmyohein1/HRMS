using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRMS.Models.Views.Setup;

namespace HRMS.Models.View
{
    public class JobHistoryModel
    {
        public int Id { get; set; }

        //  Required EmployeeId
        [Required(ErrorMessage = "EmployeeId is required.")]
        public int EmployeeId { get; set; }

        //  Optional ManagerId (Can be null)
        public int? ManagerId { get; set; }

        //  Required JobRoleId
        [Required(ErrorMessage = "JobRoleId is required.")]
        public int JobRoleId { get; set; }

        //  Required StartDate
        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        //  Optional EndDate
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        //  Required Status
        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status must be a maximum of 50 characters.")]
        public string Status { get; set; } = string.Empty; // Consider using an Enum

        //  Optional Comments
        [StringLength(500, ErrorMessage = "Comments must be a maximum of 500 characters.")]
        public string Comments { get; set; } = string.Empty;

        //  Required DepartmentId
        [Required(ErrorMessage = "DepartmentId is required.")]
        public int DepartmentId { get; set; }

        //  Display-Only Fields
        public string? ManagerName { get; set; }
        public string? EmployeeName { get; set; }

        public string? Title { get; set; }

        //  Lists for Dropdowns or Selection
        public List<EmployeeModel>? Employees { get; set; } = new List<EmployeeModel>();
        public List<JobRoleModel>? JobRoles { get; set; } = new List<JobRoleModel>();
    }
}
