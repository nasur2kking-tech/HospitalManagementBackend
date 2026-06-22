using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IDoctorScheduleRepository
    {
        Task<(IEnumerable<DoctorSchedule> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<DoctorSchedule?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default);

        Task AddAsync(
            DoctorSchedule schedule,
            CancellationToken ct = default);

        Task UpdateAsync(
            DoctorSchedule schedule,
            CancellationToken ct = default);

        Task DeleteAsync(
            DoctorSchedule schedule,
            CancellationToken ct = default);
    }
}