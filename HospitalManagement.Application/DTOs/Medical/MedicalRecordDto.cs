using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Medical
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Diagnosis { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Prescription { get; set; } = string.Empty;

        [Required]
        [Url]
        [StringLength(500)]
        public string ReportUrl { get; set; } = string.Empty;
    }
}