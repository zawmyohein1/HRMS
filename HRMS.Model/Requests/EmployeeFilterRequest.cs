namespace HRMS.Models.Requests
{
    public class EmployeeFilterRequest
    {
        public int? EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public int? DepartmentId { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
