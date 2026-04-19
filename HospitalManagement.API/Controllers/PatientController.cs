using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Application.Common;

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
        // 🔹 GET ALL PATIENTS
        // =========================
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var patients = await _service.GetAllAsync(pagination);

            return Ok(new
            {
                success = true,
                data = patients
            });
        }

        // =========================
        // 🔹 GET PATIENT BY ID
        // =========================
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> Get(int id)
        {
            var patient = await _service.GetByIdAsync(id);

            if (patient == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Patient not found"
                });
            }

            return Ok(new
            {
                success = true,
                data = patient
            });
        }

        // =========================
        // 🔹 CREATE PATIENT
        // =========================
        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
        {
            try
            {
                var createdPatient = await _service.CreateAsync(dto);

                return CreatedAtAction(nameof(Get), new { id = createdPatient.Id }, new
                {
                    success = true,
                    message = "Patient created successfully",
                    data = createdPatient
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    error = ex.InnerException?.Message
                });
            }
        }

        // =========================
        // 🔹 UPDATE PATIENT
        // =========================
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto dto)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);

                if (existing == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Patient not found"
                    });
                }

                await _service.UpdateAsync(id, dto);

                return Ok(new
                {
                    success = true,
                    message = "Patient updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    error = ex.InnerException?.Message
                });
            }
        }

        // =========================
        // 🔹 DELETE PATIENT
        // =========================
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);

                if (existing == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Patient not found"
                    });
                }

                await _service.DeleteAsync(id);

                return Ok(new
                {
                    success = true,
                    message = "Patient deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    error = ex.InnerException?.Message
                });
            }
        }
    }
}