using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations
{
    public class DoctorScheduleRepository(
        ApplicationDbContext context) : IDoctorScheduleRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<(IEnumerable<DoctorSchedule> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.DoctorSchedules
                .Include(ds => ds.Doctor)
                .AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(ds => ds.AvailableDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<DoctorSchedule?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.DoctorSchedules
                .Include(ds => ds.Doctor)
                .AsNoTracking()
                .FirstOrDefaultAsync(ds => ds.Id == id, ct);
        }

        public async Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            return await _context.DoctorSchedules
                .Where(ds => ds.DoctorId == doctorId)
                .OrderBy(ds => ds.AvailableDate)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.DoctorSchedules
                .AnyAsync(ds => ds.Id == id, ct);
        }

        public async Task AddAsync(
            DoctorSchedule schedule,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(schedule);

            await _context.DoctorSchedules.AddAsync(schedule, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            DoctorSchedule schedule,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(schedule);

            _context.DoctorSchedules.Update(schedule);

            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(
            DoctorSchedule schedule,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(schedule);

            _context.DoctorSchedules.Remove(schedule);

            await _context.SaveChangesAsync(ct);
        }
    }
}