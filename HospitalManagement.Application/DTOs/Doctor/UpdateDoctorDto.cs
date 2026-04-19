using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Doctor
{
    public class UpdateDoctorDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;


    [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Specialization { get; set; } = string.Empty;

        public string? Qualification { get; set; }

        [Phone]
        public string? Phone { get; set; }
    }


}
