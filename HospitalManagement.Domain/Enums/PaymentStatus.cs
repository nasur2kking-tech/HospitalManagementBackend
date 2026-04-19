namespace HospitalManagement.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Cancelled = 3 // 🔥 REQUIRED (matches API)
    }
}
