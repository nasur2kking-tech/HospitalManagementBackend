using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities
{
    public class Bill : AuditableEntity
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string InvoiceNumber { get; set; }
            = string.Empty;

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }
            = PaymentStatus.Pending;

        public string? PaymentMethod { get; set; }

        public string? TransactionId { get; set; }

        public DateTime? PaidDate { get; set; }

        public Patient Patient { get; set; }
            = null!;
    }
}