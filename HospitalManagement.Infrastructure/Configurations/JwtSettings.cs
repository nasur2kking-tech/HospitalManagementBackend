namespace HospitalManagement.Infrastructure.Configurations
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        public int ExpiryMinutes { get; set; } = 60; // ✅ Default 1 hour

        // 🔥 VALIDATION METHOD (IMPORTANT)
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(SecretKey))
                throw new ApplicationException("JWT SecretKey is missing.");

            if (SecretKey.Length < 32)
                throw new ApplicationException("JWT SecretKey must be at least 32 characters.");

            if (string.IsNullOrWhiteSpace(Issuer))
                throw new ApplicationException("JWT Issuer is missing.");

            if (string.IsNullOrWhiteSpace(Audience))
                throw new ApplicationException("JWT Audience is missing.");

            if (ExpiryMinutes <= 0)
                throw new ApplicationException("JWT ExpiryMinutes must be greater than 0.");
        }
    }
}