using FluentValidation;

namespace Edemo.Application.TopUps.Commands.CreateBeneficiary;

public class CreateBeneficiaryCommandValidator : AbstractValidator<CreateBeneficiaryCommand>
{
    public CreateBeneficiaryCommandValidator()
    {
        RuleFor(x => x.Nickname).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PhoneNumber).NotEmpty();
    }
}