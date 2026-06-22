using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.DoctorSchedule
{
    public class UpdateDoctorScheduleDto
    {
        [Required]
        public DateTime AvailableDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string TimeSlot { get; set; } = string.Empty;
    }
}