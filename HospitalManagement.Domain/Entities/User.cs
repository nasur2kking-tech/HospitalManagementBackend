using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        // 🔐 Stored hashed password
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔐 Password Reset
        public string? ResetToken { get; set; }

        public DateTime? ResetTokenExpiry { get; set; }

        // 🔄 Refresh Token
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }

        // 🗑️ Soft Delete
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // 🔗 Navigation
        public Patient? Patient { get; set; }

        public Doctor? Doctor { get; set; }
    }
}