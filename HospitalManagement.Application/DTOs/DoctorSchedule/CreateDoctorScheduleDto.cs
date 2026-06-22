using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.DoctorSchedule
{
    public class CreateDoctorScheduleDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime AvailableDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string TimeSlot { get; set; } = string.Empty;
    }
}