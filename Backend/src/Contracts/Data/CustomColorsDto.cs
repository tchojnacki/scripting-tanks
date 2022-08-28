namespace Backend.Contracts.Data;

public record CustomColorsDto
{
    public IReadOnlyList<string> Colors { get; init; } = default!;
}
