using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Application.Common;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var users = await _service.GetAllAsync(pagination);

            return Ok(new
            {
                success = true,
                data = users
            });
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _service.GetByIdAsync(id);

            if (user == null)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new
            {
                success = true,
                data = user
            });
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _service.GetByIdAsync(id);

            if (user == null)
                return NotFound(new { success = false, message = "User not found" });

            await _service.DeleteAsync(id);

            return Ok(new
            {
                success = true,
                message = "User deleted successfully"
            });
        }
    }
}