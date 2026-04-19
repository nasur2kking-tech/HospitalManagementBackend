using FluentValidation;
using HospitalManagement.Application.DTOs.Billing;

namespace HospitalManagement.Application.Validators
{
    public class BillingValidator : AbstractValidator<CreateBillDto>
    {
        public BillingValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0);

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}


