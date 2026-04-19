using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HospitalManagement.Application.DTOs.Billing;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Application.Common;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _service;

        public BillingController(IBillingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all bills
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var bills = await _service.GetAllAsync(pagination);

            return Ok(new
            {
                success = true,
                data = bills
            });
        }

        /// <summary>
        /// Get bill by ID
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Receptionist,Patient")]
        public async Task<IActionResult> Get(int id)
        {
            var bill = await _service.GetByIdAsync(id);

            if (bill == null)
                return NotFound(new { success = false, message = "Bill not found" });

            return Ok(new
            {
                success = true,
                data = bill
            });
        }

        /// <summary>
        /// Create bill
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Create([FromBody] CreateBillDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { id = created.Id }, new
            {
                success = true,
                message = "Bill created successfully",
                data = created
            });
        }

        /// <summary>
        /// Update bill status
        /// </summary>
        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            try
            {
                await _service.UpdateStatusAsync(id, status);

                return Ok(new
                {
                    success = true,
                    message = "Bill status updated"
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

        /// <summary>
        /// Delete bill
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                success = true,
                message = "Bill deleted successfully"
            });
        }
    }
}