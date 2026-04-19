using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IBillingRepository
    {
        Task<(IEnumerable<Bill>, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct);

        Task<Bill?> GetByIdAsync(int id, CancellationToken ct);

        Task AddAsync(Bill bill, CancellationToken ct);
        Task UpdateAsync(Bill bill, CancellationToken ct);

        Task DeleteAsync(int id, CancellationToken ct); // 🔥 ADD THIS

        Task<bool> ExistsAsync(int id, CancellationToken ct);
    }
}