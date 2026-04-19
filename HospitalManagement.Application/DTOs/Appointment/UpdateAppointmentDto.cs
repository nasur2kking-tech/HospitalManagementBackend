using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Appointment
{
    public class UpdateAppointmentDto
    {
        [Required(ErrorMessage = "Appointment date is required")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Time slot is required")]
        [StringLength(50, ErrorMessage = "Time slot too long")]
        public string TimeSlot { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("Scheduled|Completed|Cancelled",
            ErrorMessage = "Status must be Scheduled, Completed, or Cancelled")]
        public string Status { get; set; } = string.Empty;
    }
}