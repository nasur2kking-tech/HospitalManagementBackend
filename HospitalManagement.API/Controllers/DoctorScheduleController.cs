using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.DoctorSchedule;
using HospitalManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorScheduleService _service;

        public DoctorScheduleController(
            IDoctorScheduleService service)
        {
            _service = service;
        }

        // ====================================
        // GET ALL
        // ====================================
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationParams pagination,
            CancellationToken ct)
        {
            var result = await _service.GetAllAsync(
                pagination,
                ct);

            return Ok(result);
        }

        // ====================================
        // GET BY ID
        // ====================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(
                id,
                ct);

            return Ok(result);
        }

        // ====================================
        // GET BY DOCTOR ID
        // ====================================
        [HttpGet("doctor/{doctorId:int}")]
        public async Task<IActionResult> GetByDoctorId(
            int doctorId,
            CancellationToken ct)
        {
            var result = await _service.GetByDoctorIdAsync(
                doctorId,
                ct);

            return Ok(result);
        }

        // ====================================
        // CREATE
        // ====================================
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDoctorScheduleDto dto,
            CancellationToken ct)
        {
            var result = await _service.CreateAsync(
                dto,
                ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        // ====================================
        // UPDATE
        // ====================================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateDoctorScheduleDto dto,
            CancellationToken ct)
        {
            await _service.UpdateAsync(
                id,
                dto,
                ct);

            return NoContent();
        }

        // ====================================
        // DELETE
        // ====================================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            await _service.DeleteAsync(
                id,
                ct);

            return NoContent();
        }

        // ====================================
        // EXISTS
        // ====================================
        [HttpGet("{id:int}/exists")]
        public async Task<IActionResult> Exists(
            int id,
            CancellationToken ct)
        {
            var exists = await _service.ExistsAsync(
                id,
                ct);

            return Ok(exists);
        }
    }
}