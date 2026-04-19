namespace HospitalManagement.Domain.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string Diagnosis { get; set; } = string.Empty;
        public string Prescription { get; set; } = string.Empty;

        // 🔥 FIXED: must match DTO (required)
        public string ReportUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Patient Patient { get; set; } = null!;
    }
}