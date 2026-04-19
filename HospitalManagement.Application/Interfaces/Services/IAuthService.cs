using HospitalManagement.Application.DTOs.Auth;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken ct = default);
        Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default);

        Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
    }
}

