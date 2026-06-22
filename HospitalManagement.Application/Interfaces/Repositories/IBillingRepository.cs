using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IBillingRepository
    {
        Task<(IEnumerable<Bill> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<Bill?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<IEnumerable<Bill>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default);

        Task AddAsync(
            Bill bill,
            CancellationToken ct = default);

        Task UpdateAsync(
            Bill bill,
            CancellationToken ct = default);

        Task DeleteAsync(
            int id,
            CancellationToken ct = default);
    }
}