namespace HospitalManagement.Domain.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string Diagnosis { get; set; } = string.Empty;

        public string Prescription { get; set; } = string.Empty;

        // ✅ File Storage
        public string? ReportUrl { get; set; }

        public string? FileName { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        // 🗑️ Soft Delete
        public bool IsDeleted { get; set; }
            = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation
        public Patient Patient { get; set; } = null!;
    }
}