using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IDoctorRepository
    {
        Task<(IEnumerable<Doctor> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<Doctor?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<Doctor?> GetByUserIdAsync(
            int userId,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default);

        Task AddAsync(
            Doctor doctor,
            CancellationToken ct = default);

        Task UpdateAsync(
            Doctor doctor,
            CancellationToken ct = default);

        Task SoftDeleteAsync(
            Doctor doctor,
            CancellationToken ct = default);
    }
}