using FluentValidation;
using HospitalManagement.Application.DTOs.Patient;

namespace HospitalManagement.Application.Validators
{
    public class UpdatePatientValidator : AbstractValidator<UpdatePatientDto>
    {
        public UpdatePatientValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            When(x => !string.IsNullOrWhiteSpace(x.Gender), () =>
            {
                RuleFor(x => x.Gender)
                    .MaximumLength(10);
            });

            When(x => x.DateOfBirth != default, () =>
            {
                RuleFor(x => x.DateOfBirth)
                    .LessThan(DateTime.UtcNow);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
            {
                RuleFor(x => x.Phone)
                    .Matches(@"^[6-9]\d{9}$");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Address), () =>
            {
                RuleFor(x => x.Address)
                    .MaximumLength(250);
            });
        }
    }
}
