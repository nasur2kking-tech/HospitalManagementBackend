using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<User>, int)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Users
                .Where(u => !u.IsDeleted)
                .AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<User?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Id == id && !u.IsDeleted,
                    ct);
        }

        public async Task<User?> GetByEmailAsync(
            string email,
            CancellationToken ct = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Email == email && !u.IsDeleted,
                    ct);
        }

        public async Task<User?> GetByResetTokenAsync(
            string token,
            CancellationToken ct = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    u => u.ResetToken == token && !u.IsDeleted,
                    ct);
        }

        public async Task<User?> GetByRefreshTokenAsync(
            string refreshToken,
            CancellationToken ct = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    u => u.RefreshToken == refreshToken &&
                         !u.IsDeleted,
                    ct);
        }

        public async Task<bool> ExistsByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Users
                .AnyAsync(
                    u => u.Id == id &&
                         !u.IsDeleted,
                    ct);
        }

        public async Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken ct = default)
        {
            return await _context.Users
                .AnyAsync(
                    u => u.Email == email &&
                         !u.IsDeleted,
                    ct);
        }

        public async Task AddAsync(
            User user,
            CancellationToken ct = default)
        {
            await _context.Users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            User user,
            CancellationToken ct = default)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(ct);
        }

        public async Task SoftDeleteAsync(
            User user,
            CancellationToken ct = default)
        {
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            _context.Users.Update(user);

            await _context.SaveChangesAsync(ct);
        }
    }
}