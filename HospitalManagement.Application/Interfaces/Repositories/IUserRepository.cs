using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<(IEnumerable<User>, int)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<User?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<User?> GetByEmailAsync(
            string email,
            CancellationToken ct = default);

        Task<User?> GetByResetTokenAsync(
            string token,
            CancellationToken ct = default);

        Task<User?> GetByRefreshTokenAsync(
            string refreshToken,
            CancellationToken ct = default);

        Task<bool> ExistsByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken ct = default);

        Task AddAsync(
            User user,
            CancellationToken ct = default);

        Task UpdateAsync(
            User user,
            CancellationToken ct = default);

        // Soft Delete
        Task SoftDeleteAsync(
            User user,
            CancellationToken ct = default);
    }
}