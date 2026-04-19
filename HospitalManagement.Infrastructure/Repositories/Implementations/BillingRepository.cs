using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations
{
    public class BillingRepository : IBillingRepository
    {
        private readonly ApplicationDbContext _context;

        public BillingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 GET ALL (WITH PAGINATION)
        public async Task<(IEnumerable<Bill>, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Bills
                .Include(b => b.Patient)
                .AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        // 🔹 GET BY ID
        public async Task<Bill?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Bills
                .Include(b => b.Patient)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        // 🔹 GET BY PATIENT
        public async Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            return await _context.Bills
                .Where(b => b.PatientId == patientId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // 🔹 EXISTS
        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Bills
                .AnyAsync(b => b.Id == id, ct);
        }

        // 🔹 CREATE
        public async Task AddAsync(Bill bill, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(bill);

            await _context.Bills.AddAsync(bill, ct);
            await _context.SaveChangesAsync(ct);
        }

        // 🔹 UPDATE
        public async Task UpdateAsync(Bill bill, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(bill);

            _context.Bills.Update(bill);
            await _context.SaveChangesAsync(ct);
        }

        // 🔥 DELETE (MISSING → NOW FIXED)
        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var bill = await _context.Bills.FindAsync(new object[] { id }, ct);

            if (bill == null)
                throw new Exception("Bill not found");

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync(ct);
        }
    }
}