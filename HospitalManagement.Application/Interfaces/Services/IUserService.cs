using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.User;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<PagedResult<UserDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default);

        Task<UserDto> GetByIdAsync(int id, CancellationToken ct = default);

        Task<bool> ExistsAsync(int id, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
