using System.Text.RegularExpressions;
using FluentValidation;
using Backend.Contracts.Data;

namespace Backend.Validation;

public class TankColorsDtoValidator : AbstractValidator<TankColorsDto>
{
    private static readonly Regex ColorRegex = new(
        @"^#[0-9a-f]{6}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public TankColorsDtoValidator()
    {
        RuleFor(x => x.TankColor).Must(BeAValidColor);
        RuleFor(x => x.TurretColor).Must(BeAValidColor);
    }

    private bool BeAValidColor(string color) => ColorRegex.IsMatch(color);
}
