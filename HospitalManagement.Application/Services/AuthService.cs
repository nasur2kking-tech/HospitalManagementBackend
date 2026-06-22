using HospitalManagement.Application.DTOs.Auth;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtGenerator _jwt;

        public AuthService(
            IUserRepository userRepo,
            IPasswordHasher hasher,
            IJwtGenerator jwt)
        {
            _userRepo = userRepo;
            _hasher = hasher;
            _jwt = jwt;
        }

        // =========================
        // REGISTER
        // =========================
        public async Task<AuthResponseDto> RegisterAsync(
            RegisterDto dto,
            CancellationToken ct = default)
        {
            var email = dto.Email.Trim().ToLower();

            if (await _userRepo.ExistsByEmailAsync(email, ct))
                throw new ApplicationException("User already exists.");

            UserRole role = UserRole.Patient;

            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                if (!Enum.TryParse(dto.Role, true, out role))
                    throw new ApplicationException("Invalid role.");
            }

            var user = new User
            {
                Name = dto.Name.Trim(),
                Email = email,
                PasswordHash = _hasher.HashPassword(dto.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepo.AddAsync(user, ct);

            var accessToken = _jwt.GenerateToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user, ct);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = _jwt.GetExpiry(),
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        // =========================
        // LOGIN
        // =========================
        public async Task<AuthResponseDto> LoginAsync(
            LoginDto dto,
            CancellationToken ct = default)
        {
            var email = dto.Email.Trim().ToLower();

            var user = await _userRepo.GetByEmailAsync(email, ct);

            if (user == null ||
                !_hasher.VerifyPassword(dto.Password, user.PasswordHash))
            {
                throw new ApplicationException("Invalid email or password.");
            }

            var accessToken = _jwt.GenerateToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user, ct);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = _jwt.GetExpiry(),
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        public async Task<AuthResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto dto,
            CancellationToken ct = default)
        {
            var user = await _userRepo.GetByRefreshTokenAsync(
                dto.RefreshToken,
                ct);

            if (user == null)
                throw new ApplicationException("Invalid refresh token.");

            if (user.RefreshTokenExpiry == null ||
                user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                throw new ApplicationException("Refresh token expired.");
            }

            var accessToken = _jwt.GenerateToken(user);
            var newRefreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user, ct);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiry = _jwt.GetExpiry(),
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        // =========================
        // LOGOUT
        // =========================
        public async Task LogoutAsync(
            string refreshToken,
            CancellationToken ct = default)
        {
            var user = await _userRepo.GetByRefreshTokenAsync(
                refreshToken,
                ct);

            if (user == null)
                return;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _userRepo.UpdateAsync(user, ct);
        }

        // =========================
        // EMAIL EXISTS
        // =========================
        public async Task<bool> EmailExistsAsync(
            string email,
            CancellationToken ct = default)
        {
            return await _userRepo.ExistsByEmailAsync(
                email.Trim().ToLower(),
                ct);
        }

        // =========================
        // FORGOT PASSWORD
        // =========================
        public async Task<string> ForgotPasswordAsync(
            string email,
            CancellationToken ct = default)
        {
            var user = await _userRepo.GetByEmailAsync(
                email.Trim().ToLower(),
                ct);

            if (user == null)
                throw new ApplicationException("User not found.");

            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            await _userRepo.UpdateAsync(user, ct);

            return user.ResetToken!;
        }

        // =========================
        // RESET PASSWORD
        // =========================
        public async Task<bool> ResetPasswordAsync(
            string token,
            string newPassword,
            CancellationToken ct = default)
        {
            var user = await _userRepo.GetByResetTokenAsync(
                token,
                ct);

            if (user == null ||
                user.ResetTokenExpiry == null ||
                user.ResetTokenExpiry <= DateTime.UtcNow)
            {
                throw new ApplicationException(
                    "Invalid or expired token.");
            }

            user.PasswordHash = _hasher.HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepo.UpdateAsync(user, ct);

            return true;
        }
    }
}