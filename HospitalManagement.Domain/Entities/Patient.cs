using HospitalManagement.Domain.Common;

namespace HospitalManagement.Domain.Entities
{
    public class Patient : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
            = string.Empty;

        public int UserId { get; set; }

        public string Gender { get; set; }
            = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; }
            = string.Empty;

        public string Address { get; set; }
            = string.Empty;

        // Calculated Property
        public int Age =>
            DateTime.Today.Year - DateOfBirth.Year -
            (DateOfBirth.Date > DateTime.Today.AddYears(
                -(DateTime.Today.Year - DateOfBirth.Year)) ? 1 : 0);

        // Navigation
        public User User { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public ICollection<MedicalRecord> MedicalRecords { get; set; }
            = new List<MedicalRecord>();

        public ICollection<Bill> Bills { get; set; }
            = new List<Bill>();
    }
}