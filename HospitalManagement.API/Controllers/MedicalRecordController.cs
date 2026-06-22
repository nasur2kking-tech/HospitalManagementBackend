using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HospitalManagement.Application.DTOs.Medical;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Application.Common;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _service;

        public MedicalRecordController(IMedicalRecordService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all medical records
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var records = await _service.GetAllAsync(pagination);

            return Ok(new
            {
                success = true,
                data = records
            });
        }

        /// <summary>
        /// Get medical record by ID
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> Get(int id)
        {
            var record = await _service.GetByIdAsync(id);

            if (record == null)
                return NotFound(new
                {
                    success = false,
                    message = "Medical record not found"
                });

            return Ok(new
            {
                success = true,
                data = record
            });
        }

        /// <summary>
        /// Create medical record (Doctor only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Create([FromBody] CreateMedicalRecordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { id = created.Id }, new
            {
                success = true,
                message = "Medical record created",
                data = created
            });
        }

        /// <summary>
        /// Upload medical report file
        /// </summary>
        [HttpPost("upload-report")]
        [Authorize(Roles = "Admin,Doctor")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadReport(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No file uploaded"
                });
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "File size exceeds limit (5MB)"
                });
            }

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads"
            );

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/Uploads/{fileName}";

            return Ok(new
            {
                success = true,
                fileName,
                url = fileUrl
            });
        }

        /// <summary>
        /// Delete medical record
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _service.GetByIdAsync(id);

            if (record == null)
                return NotFound(new
                {
                    success = false,
                    message = "Medical record not found"
                });

            await _service.DeleteAsync(id);

            return Ok(new
            {
                success = true,
                message = "Medical record deleted"
            });
        }
    }
}