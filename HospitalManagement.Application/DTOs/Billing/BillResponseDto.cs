using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Billing
{
    public class BillResponseDto
    {
        public int Id { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression("Pending|Paid|Cancelled",
            ErrorMessage = "Status must be Pending, Paid, or Cancelled")]
        public string Status { get; set; } = string.Empty;
    }
}