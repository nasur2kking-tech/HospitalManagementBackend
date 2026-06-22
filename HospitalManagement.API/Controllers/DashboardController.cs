using HospitalManagement.Domain.Enums;
using HospitalManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var totalPatients =
                await _context.Patients.CountAsync(x => !x.IsDeleted);

            var totalDoctors =
                await _context.Doctors.CountAsync(x => !x.IsDeleted);

            var totalAppointments =
                await _context.Appointments.CountAsync(x => !x.IsDeleted);

            var pendingBills =
                await _context.Bills.CountAsync(
                    x => x.Status == PaymentStatus.Pending &&
                         !x.IsDeleted);

            var totalRevenue =
                await _context.Bills
                    .Where(x =>
                        x.Status == PaymentStatus.Paid &&
                        !x.IsDeleted)
                    .SumAsync(x => (decimal?)x.Amount)
                    ?? 0;

            return Ok(new
            {
                success = true,
                data = new
                {
                    totalPatients,
                    totalDoctors,
                    totalAppointments,
                    pendingBills,
                    totalRevenue
                }
            });
        }
    }
}