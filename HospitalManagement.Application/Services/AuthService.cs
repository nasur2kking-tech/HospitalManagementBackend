using HospitalManagement.Application.DTOs.Auth;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.Services;

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
    // 🔹 REGISTER
    // =========================
    public async Task<AuthResponseDto> RegisterAsync(
        RegisterDto dto,
        CancellationToken ct = default)
    {
        if (dto == null)
            throw new ApplicationException("Invalid request.");

        var email = dto.Email?.Trim().ToLower();
        var name = dto.Name?.Trim();

        if (string.IsNullOrWhiteSpace(email))
            throw new ApplicationException("Email is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ApplicationException("Name is required.");

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
            throw new ApplicationException("Password must be at least 6 characters.");

        // 🔥 CHECK EXISTING USER
        if (await _userRepo.ExistsByEmailAsync(email, ct))
            throw new ApplicationException("User already exists.");

        // 🔥 ROLE VALIDATION
        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            throw new ApplicationException("Invalid role. Use Admin, Doctor, Receptionist.");

        // 🔥 CREATE USER
        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = _hasher.HashPassword(dto.Password),
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepo.AddAsync(user, ct);

        // 🔥 GENERATE TOKEN
        var token = _jwt.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    // =========================
    // 🔹 LOGIN
    // =========================
    public async Task<AuthResponseDto> LoginAsync(
        LoginDto dto,
        CancellationToken ct = default)
    {
        if (dto == null)
            throw new ApplicationException("Invalid request.");

        var email = dto.Email?.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(email))
            throw new ApplicationException("Email is required.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ApplicationException("Password is required.");

        var user = await _userRepo.GetByEmailAsync(email, ct);

        if (user == null)
            throw new ApplicationException("User not found.");

        if (!_hasher.VerifyPassword(dto.Password, user.PasswordHash))
            throw new ApplicationException("Invalid password.");

        var token = _jwt.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    // =========================
    // 🔹 CHECK EMAIL EXISTS
    // =========================
    public async Task<bool> EmailExistsAsync(
        string email,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return await _userRepo.ExistsByEmailAsync(
            email.Trim().ToLower(),
            ct
        );
    }
}