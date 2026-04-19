using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

    public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Doctor>, int)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Doctors
                .Include(d => d.User)
                .AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<Doctor?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task<Doctor?> GetByUserIdAsync(int userId, CancellationToken ct = default)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId, ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Doctors.AnyAsync(d => d.Id == id, ct);
        }

        public async Task AddAsync(Doctor doctor, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(doctor);

            await _context.Doctors.AddAsync(doctor, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Doctor doctor, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(doctor);

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Doctor doctor, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(doctor);

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync(ct);
        }
    }


}
