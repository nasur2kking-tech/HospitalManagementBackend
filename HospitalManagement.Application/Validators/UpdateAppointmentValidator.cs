using FluentValidation;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.Validators
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            When(x => x.AppointmentDate != default, () =>
            {
                RuleFor(x => x.AppointmentDate)
                    .Must(date => date.Date >= DateTime.UtcNow.Date);
            });

            When(x => !string.IsNullOrWhiteSpace(x.TimeSlot), () =>
            {
                RuleFor(x => x.TimeSlot)
                    .MaximumLength(50);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Status), () =>
            {
                RuleFor(x => x.Status)
                    .Must(s => Enum.TryParse<AppointmentStatus>(s, true, out _))
                    .WithMessage("Invalid appointment status.");
            });
        }
    }
}
