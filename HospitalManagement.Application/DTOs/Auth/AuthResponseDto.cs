using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;

        [Required]
        public DateTime AccessTokenExpiry { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Admin|Doctor|Patient|Receptionist")]
        public string Role { get; set; } = string.Empty;
    }
}