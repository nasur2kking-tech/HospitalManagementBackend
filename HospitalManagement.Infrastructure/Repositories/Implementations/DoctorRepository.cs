using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations
{
    public class DoctorRepository(ApplicationDbContext context)
        : IDoctorRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<(IEnumerable<Doctor> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Doctors
                .Include(d => d.User)
                .AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<Doctor?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task<Doctor?> GetByUserIdAsync(
            int userId,
            CancellationToken ct = default)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == userId, ct);
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Doctors
                .AnyAsync(d => d.Id == id, ct);
        }

        public async Task AddAsync(
            Doctor doctor,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(doctor);

            await _context.Doctors.AddAsync(doctor, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            Doctor doctor,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(doctor);

            _context.Doctors.Update(doctor);

            await _context.SaveChangesAsync(ct);
        }

        public async Task SoftDeleteAsync(
            Doctor doctor,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(doctor);

            doctor.IsDeleted = true;
            doctor.DeletedAt = DateTime.UtcNow;

            _context.Doctors.Update(doctor);

            await _context.SaveChangesAsync(ct);
        }
    }
}