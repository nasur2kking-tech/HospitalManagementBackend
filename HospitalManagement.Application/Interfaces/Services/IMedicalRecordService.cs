using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Medical;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IMedicalRecordService
    {
        Task<PagedResult<MedicalRecordDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
        Task<MedicalRecordDto> GetByIdAsync(int id, CancellationToken ct = default);

        Task<MedicalRecordDto> CreateAsync(CreateMedicalRecordDto dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    }
}

