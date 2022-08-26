namespace Backend.Contracts.Data;

public record CustomColorsDto
{
    public List<string> Colors { get; init; } = default!;
}
