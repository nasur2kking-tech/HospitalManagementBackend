using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Patient>, int)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Patients
                .Include(p => p.User)
                .AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<Patient?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<Patient?> GetByUserIdAsync(int userId, CancellationToken ct = default)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId, ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Patients.AnyAsync(p => p.Id == id, ct);
        }

        public async Task AddAsync(Patient patient, CancellationToken ct = default)
        {
            await _context.Patients.AddAsync(patient, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Patient patient, CancellationToken ct = default)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Patient patient, CancellationToken ct = default)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync(ct);
        }
    }
}