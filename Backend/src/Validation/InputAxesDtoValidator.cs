using FluentValidation;
using Backend.Contracts.Data;

namespace Backend.Validation;

public class InputAxesDtoValidator : AbstractValidator<InputAxesDto>
{
    public InputAxesDtoValidator()
    {
        RuleFor(x => x.Vertical).Must(BeWithinAxisRange);
        RuleFor(x => x.Horizontal).Must(BeWithinAxisRange);
    }

    private bool BeWithinAxisRange(double axis) => axis is >= -1 and <= 1;
}
