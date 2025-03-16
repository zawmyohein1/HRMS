using Microsoft.AspNetCore.Mvc;
using HRMS.Model.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models.Views.Setup;
using HRMS.Services.Cores.Interfaces.Setup;

namespace HRMS.API.Controllers.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/Department
        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var response = await _departmentService.GetsAsync();
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/Department/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _departmentService.GetAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/Department/Create
        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            var response = await _departmentService.GetDepartmentAsync();
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/Department
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentModel departmentDto)
        {
            var response = await _departmentService.CreateAsync(departmentDto);
            return StatusCode(response.StatusCode, response);
        }

        // PUT: api/Department/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentModel departmentDto)
        {            
            var response = await _departmentService.UpdateAsync(departmentDto);
            return StatusCode(response.StatusCode, response);
        }

        // DELETE: api/Department/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _departmentService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
