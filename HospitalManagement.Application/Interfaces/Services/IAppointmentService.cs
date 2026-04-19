using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Appointment;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<PagedResult<AppointmentResponseDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default);

        Task<AppointmentResponseDto> GetByIdAsync(int id, CancellationToken ct = default);

        Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto, CancellationToken ct = default);

        Task UpdateAsync(int id, UpdateAppointmentDto dto, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    }
}
