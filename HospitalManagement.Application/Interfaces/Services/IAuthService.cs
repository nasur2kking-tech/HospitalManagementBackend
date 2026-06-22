using HospitalManagement.Application.DTOs.Auth;

namespace HospitalManagement.Application.Interfaces.Services
{
    public interface IAuthService
    {
        // =========================
        // 🔹 REGISTER
        // =========================
        Task<AuthResponseDto> RegisterAsync(
            RegisterDto dto,
            CancellationToken ct = default);

        // =========================
        // 🔹 LOGIN
        // =========================
        Task<AuthResponseDto> LoginAsync(
            LoginDto dto,
            CancellationToken ct = default);

        // =========================
        // 🔹 REFRESH TOKEN
        // =========================
        Task<AuthResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto dto,
            CancellationToken ct = default);

        // =========================
        // 🔹 LOGOUT
        // =========================
        Task LogoutAsync(
            string refreshToken,
            CancellationToken ct = default);

        // =========================
        // 🔹 CHECK EMAIL EXISTS
        // =========================
        Task<bool> EmailExistsAsync(
            string email,
            CancellationToken ct = default);

        // =========================
        // 🔹 FORGOT PASSWORD
        // =========================
        Task<string> ForgotPasswordAsync(
            string email,
            CancellationToken ct = default);

        // =========================
        // 🔹 RESET PASSWORD
        // =========================
        Task<bool> ResetPasswordAsync(
            string token,
            string newPassword,
            CancellationToken ct = default);
    }
}