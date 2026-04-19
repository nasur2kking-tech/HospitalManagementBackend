using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Patient;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<PagedResult<PatientResponseDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
        Task<PatientResponseDto> GetByIdAsync(int id, CancellationToken ct = default);

        Task<PatientResponseDto> CreateAsync(CreatePatientDto dto, CancellationToken ct = default);
        Task UpdateAsync(int id, UpdatePatientDto dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    }
}
