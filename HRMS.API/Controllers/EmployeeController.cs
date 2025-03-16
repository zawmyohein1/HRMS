using Microsoft.AspNetCore.Mvc;
using HRMS.Models.View;
using HRMS.Models.Requests;
using HRMS.Services.Cores.Interfaces;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var response = await _employeeService.GetsAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _employeeService.GetAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> Filter([FromBody] EmployeeFilterRequest request)
        {
            var response = await _employeeService.GetFilteredEmployeesAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeModel employeeDto)
        {
            var response = await _employeeService.CreateAsync(employeeDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeModel employeeDto)
        {
            var response = await _employeeService.UpdateAsync(employeeDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _employeeService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
