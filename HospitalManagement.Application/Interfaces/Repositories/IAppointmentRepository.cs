using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<(IEnumerable<Appointment>, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct);

        Task<Appointment?> GetByIdAsync(int id, CancellationToken ct);

        Task AddAsync(Appointment appointment, CancellationToken ct);
        Task UpdateAsync(Appointment appointment, CancellationToken ct);
        Task DeleteAsync(Appointment appointment, CancellationToken ct);

        Task<bool> ExistsAsync(int id, CancellationToken ct);
    }
}
