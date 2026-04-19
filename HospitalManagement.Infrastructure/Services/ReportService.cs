using HospitalManagement.Application.DTOs.Report;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Services
{
    public class ReportService(ApplicationDbContext context) : IReportService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ReportSummaryDto> GetSummaryAsync(CancellationToken ct = default)
        {
            var totalPatients = await _context.Patients.CountAsync(ct);
            var totalDoctors = await _context.Doctors.CountAsync(ct);
            var totalAppointments = await _context.Appointments.CountAsync(ct);

            var totalRevenue = await _context.Bills
                .Where(b => b.Status == PaymentStatus.Paid)
                .SumAsync(b => (decimal?)b.Amount, ct) ?? 0m;

            return new ReportSummaryDto
            {
                TotalPatients = totalPatients,
                TotalDoctors = totalDoctors,
                TotalAppointments = totalAppointments,
                TotalRevenue = totalRevenue
            };
        }
    }
}