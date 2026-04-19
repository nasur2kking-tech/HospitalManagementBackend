using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Medical
{
    public class CreateMedicalRecordDto
    {
        [Required(ErrorMessage = "PatientId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid PatientId")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(500, ErrorMessage = "Diagnosis cannot exceed 500 characters")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prescription is required")]
        [StringLength(500, ErrorMessage = "Prescription cannot exceed 500 characters")]
        public string Prescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Report URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500, ErrorMessage = "Report URL too long")]
        public string ReportUrl { get; set; } = string.Empty;
    }
}