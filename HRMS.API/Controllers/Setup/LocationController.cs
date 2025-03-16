using Microsoft.AspNetCore.Mvc;
using HRMS.Models.Views.Setup;
using HRMS.Services.Cores.Interfaces.Setup;

namespace HRMS.API.Controllers.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var response = await _locationService.GetsAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _locationService.GetAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationModel model)
        {
            var response = await _locationService.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LocationModel model)
        {
            var response = await _locationService.UpdateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _locationService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
