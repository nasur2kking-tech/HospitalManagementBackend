using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations
{
    public class BillingRepository(ApplicationDbContext context)
        : IBillingRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<(IEnumerable<Bill> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Bills
                .Include(b => b.Patient)
                .AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(b => b.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<Bill?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Bills
                .Include(b => b.Patient)
                .FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        public async Task<IEnumerable<Bill>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await _context.Bills
                .Where(b => b.PatientId == patientId)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Bills
                .AnyAsync(b => b.Id == id, ct);
        }

        public async Task AddAsync(
            Bill bill,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(bill);

            await _context.Bills.AddAsync(bill, ct);

            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            Bill bill,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(bill);

            _context.Bills.Update(bill);

            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            var bill = await _context.Bills
                .FirstOrDefaultAsync(b => b.Id == id, ct);

            if (bill is null)
                return;

            _context.Bills.Remove(bill);

            await _context.SaveChangesAsync(ct);
        }
    }
}