using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Billing;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IBillingService
    {
        Task<PagedResult<BillResponseDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default);

        Task<BillResponseDto> GetByIdAsync(int id, CancellationToken ct = default);

        Task<BillResponseDto> CreateAsync(CreateBillDto dto, CancellationToken ct = default);

        Task UpdateStatusAsync(int id, string status, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default); // 🔥 ADD THIS

        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    }
}