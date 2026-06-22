using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.Interfaces.Services;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        // =========================
        // GET ALL PATIENTS
        // =========================
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            try
            {
                var patients = await _service.GetAllAsync(pagination);

                return Ok(new
                {
                    success = true,
                    data = patients
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =========================
        // GET PATIENT BY ID
        // =========================
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var patient = await _service.GetByIdAsync(id);

                return Ok(new
                {
                    success = true,
                    data = patient
                });
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =========================
        // CREATE PATIENT
        // =========================
        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = ModelState
                        .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Value!.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var patient = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(Get),
                    new { id = patient.Id },
                    new
                    {
                        success = true,
                        message = "Patient created successfully",
                        data = patient
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        // =========================
        // UPDATE PATIENT
        // =========================
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);

                return Ok(new
                {
                    success = true,
                    message = "Patient updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =========================
        // DELETE PATIENT
        // =========================
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return Ok(new
                {
                    success = true,
                    message = "Patient deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}