using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.View
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Birth Date is required.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone number must be a maximum of 15 characters.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email must be a maximum of 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10, ErrorMessage = "Gender must be a maximum of 10 characters.")]
        public string Gender { get; set; } = string.Empty;  // Consider using an Enum

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, ErrorMessage = "Address must be a maximum of 250 characters.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status must be a maximum of 50 characters.")]
        public string Status { get; set; } = string.Empty;  // Consider using an Enum       

        public ICollection<JobHistoryModel> JobHistories { get; set; } = new List<JobHistoryModel>();
    }
}
