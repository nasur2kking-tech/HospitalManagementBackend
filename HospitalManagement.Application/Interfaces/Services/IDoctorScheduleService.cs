using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.DoctorSchedule;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IDoctorScheduleService
    {
        Task<PagedResult<DoctorScheduleResponseDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default);

        Task<DoctorScheduleResponseDto> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<IEnumerable<DoctorScheduleResponseDto>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default);

        Task<DoctorScheduleResponseDto> CreateAsync(
            CreateDoctorScheduleDto dto,
            CancellationToken ct = default);

        Task UpdateAsync(
            int id,
            UpdateDoctorScheduleDto dto,
            CancellationToken ct = default);

        Task DeleteAsync(
            int id,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default);
    }
}