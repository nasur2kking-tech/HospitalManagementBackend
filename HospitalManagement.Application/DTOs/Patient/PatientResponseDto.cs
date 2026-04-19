using System;

namespace HospitalManagement.Application.DTOs.Patient
{
    public class PatientResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Gender { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}