using HospitalManagement.Application.DTOs.Report;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<ReportSummaryDto> GetSummaryAsync(CancellationToken ct = default);
    }
}