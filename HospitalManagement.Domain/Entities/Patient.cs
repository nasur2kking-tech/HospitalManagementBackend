namespace HospitalManagement.Domain.Entities
{
    public class Patient
    {

        public string Name { get; set; }
        public int Age { get; set; }

        public int Id { get; set; }

        public int UserId { get; set; }

        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}
