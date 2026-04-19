using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities
{
    public class Bill
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Patient Patient { get; set; } = null!;
    }
}
