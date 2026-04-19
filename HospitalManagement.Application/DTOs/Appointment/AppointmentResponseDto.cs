using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Appointment
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string TimeSlot { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Scheduled|Completed|Cancelled")]
        public string Status { get; set; } = string.Empty;
    }
}