using FluentValidation;

namespace Edemo.Application.TopUps.Commands.DeleteBeneficiary;

public class DeleteBeneficiaryCommandValidator : AbstractValidator<DeleteBeneficiaryCommand>
{
    public DeleteBeneficiaryCommandValidator()
    {
        RuleFor(x => x.BeneficiaryId).NotEmpty();
    }
}