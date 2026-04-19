using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HospitalManagement.Application.DTOs.Doctor;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Application.Common;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorsController(IDoctorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all doctors (Public)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var doctors = await _service.GetAllAsync(pagination);

            return Ok(new
            {
                success = true,
                data = doctors
            });
        }

        /// <summary>
        /// Get doctor by ID (Public)
        /// </summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return NotFound(new { success = false, message = "Doctor not found" });

            return Ok(new
            {
                success = true,
                data = doctor
            });
        }

        /// <summary>
        /// Create new doctor
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDoctor = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { id = createdDoctor.Id }, new
            {
                success = true,
                message = "Doctor created successfully",
                data = createdDoctor
            });
        }

        /// <summary>
        /// Update doctor
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);

            return Ok(new
            {
                success = true,
                message = "Doctor updated successfully"
            });
        }

        /// <summary>
        /// Delete doctor
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                success = true,
                message = "Doctor deleted successfully"
            });
        }
    }
}