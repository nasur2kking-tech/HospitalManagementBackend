using HospitalManagement.Application.DTOs.Auth;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.Services;

public class AuthService(
    IUserRepository userRepo,
    IPasswordHasher hasher,
    IJwtGenerator jwt
) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var email = dto.Email?.Trim().ToLower()
                    ?? throw new ApplicationException("Email is required.");

        var name = dto.Name?.Trim()
                   ?? throw new ApplicationException("Name is required.");

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
            throw new ApplicationException("Password must be at least 6 characters.");

        if (await userRepo.ExistsByEmailAsync(email, ct))
            throw new ApplicationException("User already exists.");

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            throw new ApplicationException("Invalid role.");

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = hasher.HashPassword(dto.Password),
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        await userRepo.AddAsync(user, ct);

        var token = jwt.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var email = dto.Email?.Trim().ToLower()
                    ?? throw new ApplicationException("Email is required.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ApplicationException("Password is required.");

        var user = await userRepo.GetByEmailAsync(email, ct);

        if (user == null || !hasher.VerifyPassword(dto.Password, user.PasswordHash))
            throw new ApplicationException("Invalid email or password.");

        var token = jwt.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return await userRepo.ExistsByEmailAsync(email.Trim().ToLower(), ct);
    }
}

