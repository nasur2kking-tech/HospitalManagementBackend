using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
