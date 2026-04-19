using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<MedicalRecord>, int)> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var query = _context.MedicalRecords
                .Include(m => m.Patient)
                .AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<MedicalRecord?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            return await _context.MedicalRecords
                .Where(m => m.PatientId == patientId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _context.MedicalRecords.AnyAsync(m => m.Id == id, ct);
        }

        public async Task AddAsync(MedicalRecord record, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(record);

            await _context.MedicalRecords.AddAsync(record, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(MedicalRecord record, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(record);

            _context.MedicalRecords.Remove(record);
            await _context.SaveChangesAsync(ct);
        }
    }
}
