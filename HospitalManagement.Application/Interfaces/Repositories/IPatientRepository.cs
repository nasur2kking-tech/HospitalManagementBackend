using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<(IEnumerable<Patient> Patients, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<Patient?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<Patient?> GetByUserIdAsync(
            int userId,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default);

        Task AddAsync(
            Patient patient,
            CancellationToken ct = default);

        Task UpdateAsync(
            Patient patient,
            CancellationToken ct = default);

        // Soft Delete
        Task SoftDeleteAsync(
            Patient patient,
            CancellationToken ct = default);
    }
}