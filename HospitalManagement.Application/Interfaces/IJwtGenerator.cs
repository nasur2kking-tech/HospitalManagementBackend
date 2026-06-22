using System.Security.Claims;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces
{
    public interface IJwtGenerator
    {
        // Access Token
        string GenerateToken(User user);

        // Access Token Expiry
        DateTime GetExpiry();

        // Refresh Token
        string GenerateRefreshToken();

        // Validate Expired Access Token
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}