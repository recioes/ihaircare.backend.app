using Core.DTOs;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

  
        [HttpPost]
        //[Authorize]
        [SwaggerOperation(Summary = "Creates a new schedule")]
        public async Task<IActionResult> CreateSchedule([FromBody] ScheduleDto schedule)
        {
            await _scheduleService.CreateScheduleAsync(schedule);
            return Ok("Schedule created");
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing schedule")]
        public async Task<IActionResult> UpdateSchedule([FromRoute] ObjectId id, [FromBody] ScheduleDto scheduleDto)
        {
            await _scheduleService.UpdateScheduleAsync(id, scheduleDto);
            return Ok("Schedule updated");
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a schedule by ID")]
        public async Task<IActionResult> DeleteScheduleAsync([FromRoute] ObjectId id)
        {
            await _scheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a schedule by its ID")]
        public async Task<IActionResult> GetScheduleById([FromRoute] ObjectId id)
        {
            var schedule = await _scheduleService.GetScheduleById(id);
            if (schedule == null)
            {
                return NotFound($"Schedule with id {id} not found.");
            }
            return Ok(schedule);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all schedules")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _scheduleService.GetScheduleAsync();
            if (!schedules.Any())
            {
                return NotFound("No schedules found.");
            }
            return Ok(schedules);
        }
    }
}
