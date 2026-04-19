using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Billing
{
    public class CreateBillDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}