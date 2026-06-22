using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IMedicalRecordRepository
    {
        Task<(IEnumerable<MedicalRecord> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<MedicalRecord?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default);

        Task AddAsync(
            MedicalRecord record,
            CancellationToken ct = default);

        Task UpdateAsync(
            MedicalRecord record,
            CancellationToken ct = default);

        Task DeleteAsync(
            MedicalRecord record,
            CancellationToken ct = default);
    }
}