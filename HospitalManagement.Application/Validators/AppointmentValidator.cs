using FluentValidation;
using HospitalManagement.Application.DTOs.Appointment;

namespace HospitalManagement.Application.Validators
{
    public class AppointmentValidator : AbstractValidator<CreateAppointmentDto>
    {
        public AppointmentValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.PatientId)
                .GreaterThan(0);

            RuleFor(x => x.DoctorId)
                .GreaterThan(0);

            RuleFor(x => x.AppointmentDate)
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                .WithMessage("Appointment must be today or future.");

            RuleFor(x => x.TimeSlot)
                .NotEmpty()
                .MaximumLength(50)
                .Must(v => !string.IsNullOrWhiteSpace(v));

            RuleFor(x => x.Reason)
                .NotEmpty()
                .MaximumLength(500)
                .Must(v => !string.IsNullOrWhiteSpace(v));
        }
    }
}
