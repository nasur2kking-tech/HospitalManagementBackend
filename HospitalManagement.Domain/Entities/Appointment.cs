using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities
{
    public class Appointment : AuditableEntity
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string TimeSlot { get; set; }
            = string.Empty;

        public AppointmentStatus Status { get; set; }
            = AppointmentStatus.Scheduled;

        public string? Reason { get; set; }

        public string? Notes { get; set; }

        public Patient Patient { get; set; }
            = null!;

        public Doctor Doctor { get; set; }
            = null!;
    }
}