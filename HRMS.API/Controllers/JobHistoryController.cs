using Microsoft.AspNetCore.Mvc;
using HRMS.Models.View;
using HRMS.Services.Cores.Interfaces;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobHistoryController : ControllerBase
    {
        private readonly IJobHistoryService _jobHistoryService;

        public JobHistoryController(IJobHistoryService jobHistoryService)
        {
            _jobHistoryService = jobHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var response = await _jobHistoryService.GetsAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _jobHistoryService.GetAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            var response = await _jobHistoryService.GetJobHistoryAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobHistoryModel jobHistoryDto)
        {
            var response = await _jobHistoryService.CreateAsync(jobHistoryDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobHistoryModel jobHistoryDto)
        {
            var response = await _jobHistoryService.UpdateAsync(jobHistoryDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _jobHistoryService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
