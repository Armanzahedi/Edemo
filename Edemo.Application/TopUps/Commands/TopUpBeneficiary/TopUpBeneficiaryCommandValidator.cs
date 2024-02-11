using FluentValidation;

namespace Edemo.Application.TopUps.Commands.TopUpBeneficiary;

public class TopUpBeneficiaryCommandValidator : AbstractValidator<TopUpBeneficiaryCommand>
{
    public TopUpBeneficiaryCommandValidator()
    {
        RuleFor(x => x.BeneficiaryId).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty();
    }
}