using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken ct);

        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<User?> GetByIdAsync(int id, CancellationToken ct);

        Task<(IEnumerable<User>, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct);

        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

        Task DeleteAsync(User user, CancellationToken ct);
    }
}
