namespace Backend.Contracts.Data;

public abstract record AbstractEntityDto
{
    public abstract string Kind { get; }
    public string Eid { get; init; } = default!;
    public IReadOnlyList<double> Pos { get; init; } = default!;
}
