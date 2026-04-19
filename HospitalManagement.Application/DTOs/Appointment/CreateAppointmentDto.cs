using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "PatientId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid PatientId")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid DoctorId")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Appointment date is required")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Time slot is required")]
        [StringLength(50, ErrorMessage = "Time slot too long")]
        public string TimeSlot { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(500, ErrorMessage = "Reason too long")]
        public string Reason { get; set; } = string.Empty;
    }
}