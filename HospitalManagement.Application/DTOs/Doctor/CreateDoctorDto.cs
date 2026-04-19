using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Doctor
{
    public class CreateDoctorDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Qualification { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public int ExperienceYears { get; set; } // ✅ ADD THIS
    }
}