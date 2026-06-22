using HospitalManagement.Application.DTOs.Auth;
using HospitalManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            return Ok(result);
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(result);
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);

            return Ok(result);
        }

        // =========================
        // LOGOUT
        // =========================
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            [FromBody] string refreshToken)
        {
            await _authService.LogoutAsync(refreshToken);

            return Ok(new
            {
                Message = "Logout successful"
            });
        }

        // =========================
        // FORGOT PASSWORD
        // =========================
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromQuery] string email)
        {
            var token = await _authService.ForgotPasswordAsync(email);

            return Ok(new
            {
                Message = "Reset token generated",
                Token = token
            });
        }

        // =========================
        // RESET PASSWORD
        // =========================
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromQuery] string token,
            [FromQuery] string newPassword)
        {
            await _authService.ResetPasswordAsync(
                token,
                newPassword);

            return Ok(new
            {
                Message = "Password reset successful"
            });
        }
    }
}