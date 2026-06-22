namespace HospitalManagement.Application.DTOs.DoctorSchedule
{
    public class DoctorScheduleResponseDto
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public DateTime AvailableDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;
    }
}