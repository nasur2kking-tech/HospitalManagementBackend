using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Doctor;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<PagedResult<DoctorResponseDto>> GetAllAsync(
        PaginationParams pagination,
        CancellationToken ct = default);

    Task<DoctorResponseDto> GetByIdAsync(int id, CancellationToken ct = default);

        Task<DoctorResponseDto> CreateAsync(CreateDoctorDto dto, CancellationToken ct = default);

        // 🔥 FIXED HERE
        Task UpdateAsync(int id, UpdateDoctorDto dto, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    }

}
