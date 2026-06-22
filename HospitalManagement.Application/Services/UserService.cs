using AutoMapper;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.User;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;

namespace HospitalManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            var (items, totalCount) = await _repo.GetAllAsync(
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<UserDto>
            {
                Items = _mapper.Map<IEnumerable<UserDto>>(items),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<UserDto> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var user = await _repo.GetByIdAsync(id, ct);

            if (user is null)
                throw new ApplicationException("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _repo.ExistsByIdAsync(id, ct);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            var user = await _repo.GetByIdAsync(id, ct);

            if (user is null)
                throw new ApplicationException("User not found.");

            // Soft Delete
            await _repo.SoftDeleteAsync(user, ct);
        }
    }
}