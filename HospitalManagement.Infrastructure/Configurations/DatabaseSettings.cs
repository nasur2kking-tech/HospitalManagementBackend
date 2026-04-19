namespace HospitalManagement.Infrastructure.Configurations
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;

        // 🔥 VALIDATION METHOD
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new ApplicationException("Database connection string is missing.");
        }
    }
}