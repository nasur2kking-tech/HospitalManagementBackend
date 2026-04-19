using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Common;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all appointments (with pagination)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            if (pagination.PageNumber <= 0 || pagination.PageSize <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid pagination parameters"
                });
            }

            var appointments = await _service.GetAllAsync(pagination);

            return Ok(new
            {
                success = true,
                data = appointments
            });
        }

        /// <summary>
        /// Get appointment by ID
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,Receptionist,Patient")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _service.GetByIdAsync(id);

            if (appointment == null)
                return NotFound(new { success = false, message = "Appointment not found" });

            return Ok(new
            {
                success = true,
                data = appointment
            });
        }

        /// <summary>
        /// Create a new appointment
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist,Patient")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });

            try
            {
                var created = await _service.CreateAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, new
                {
                    success = true,
                    message = "Appointment booked successfully",
                    data = created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message // handles double booking etc.
                });
            }
        }

        /// <summary>
        /// Update appointment (reschedule)
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);

            if (existing == null)
                return NotFound(new { success = false, message = "Appointment not found" });

            await _service.UpdateAsync(id, dto);

            return Ok(new
            {
                success = true,
                message = "Appointment updated successfully"
            });
        }

        /// <summary>
        /// Delete appointment
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);

            if (existing == null)
                return NotFound(new { success = false, message = "Appointment not found" });

            await _service.DeleteAsync(id);

            return Ok(new
            {
                success = true,
                message = "Appointment deleted successfully"
            });
        }
    }
}