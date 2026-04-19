using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Doctor
{
    public class DoctorResponseDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Qualification { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;
    }
}