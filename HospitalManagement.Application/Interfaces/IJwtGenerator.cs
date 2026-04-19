using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
        DateTime GetExpiry();
    }
}
