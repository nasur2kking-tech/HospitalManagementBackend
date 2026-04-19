using FluentValidation;
using HospitalManagement.Application.DTOs.Patient;

namespace HospitalManagement.Application.Validators
{
    public class PatientValidator : AbstractValidator<CreatePatientDto>
    {
        public PatientValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.Gender)
                .NotEmpty()
                .MaximumLength(10)
                .Must(v => !string.IsNullOrWhiteSpace(v));

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.UtcNow);

            RuleFor(x => x.Phone)
                .Matches(@"^[6-9]\d{9}$")
                .WithMessage("Invalid Indian phone number.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(250)
                .Must(v => !string.IsNullOrWhiteSpace(v));
        }
    }
}

