using FluentValidation;
using HospitalManagement.Application.DTOs.Doctor;

namespace HospitalManagement.Application.Validators
{
    public class DoctorValidator : AbstractValidator<CreateDoctorDto>
    {
        public DoctorValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.Specialization)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.ExperienceYears)
                .GreaterThanOrEqualTo(0);
        }
    }
}