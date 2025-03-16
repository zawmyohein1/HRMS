using Microsoft.AspNetCore.Mvc;
using HRMS.Models.Views.Setup;
using HRMS.Services.Cores.Interfaces.Setup;

namespace HRMS.API.Controllers.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRoleController : ControllerBase
    {
        private readonly IJobRoleService _jobRoleService;

        public JobRoleController(IJobRoleService jobRoleService)
        {
            _jobRoleService = jobRoleService;
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var response = await _jobRoleService.GetsAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _jobRoleService.GetAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/JobRole/Create
        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            var response = await _jobRoleService.GetJobRoleAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobRoleModel jobRoleDto)
        {
            var response = await _jobRoleService.CreateAsync(jobRoleDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobRoleModel jobRoleDto)
        {
            var response = await _jobRoleService.UpdateAsync(jobRoleDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _jobRoleService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
