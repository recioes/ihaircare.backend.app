using Core.DTOs;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new treatment")]
        public async Task<IActionResult> CreateTreatment([FromBody] TreatmentDto treatment)
        {
            await _treatmentService.AddTreatmentAsync(treatment);
            return Ok("Treatment created");
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all treatments")]
        public async Task<IActionResult> GetAllTreatments()
        {
            var treatments = await _treatmentService.GetAllTreatmentsAsync();
            return Ok(treatments);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets treatment by id")]
        public async Task<IActionResult> GetTreatmentById([FromRoute] ObjectId id)
        {
            var treatment = await _treatmentService.GetTreatmentByIdAsync(id);
            return Ok(treatment);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates a treatment")]
        public async Task<IActionResult> UpdateTreatment([FromRoute] ObjectId id, [FromBody] TreatmentDto treatmentDto)
        {
            await _treatmentService.UpdateTreatmentAsync(id, treatmentDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a treatment")]
        public async Task<IActionResult> DeleteTreatment([FromRoute] ObjectId id)
        {
            await _treatmentService.DeleteTreatmentAsync(id);
            return NoContent();
        }
    }
}
