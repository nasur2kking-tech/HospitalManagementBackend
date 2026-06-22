namespace HospitalManagement.Domain.Entities
{
    public class DoctorSchedule
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public DateTime AvailableDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🗑️ Soft Delete
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        // Navigation
        public Doctor Doctor { get; set; } = null!;
    }
}