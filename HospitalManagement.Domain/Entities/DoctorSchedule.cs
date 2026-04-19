namespace HospitalManagement.Domain.Entities
{
    public class DoctorSchedule
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public DateTime AvailableDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;

        // Navigation
        public Doctor Doctor { get; set; } = null!;
    }
}
