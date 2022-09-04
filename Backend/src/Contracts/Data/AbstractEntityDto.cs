namespace Backend.Contracts.Data;

public abstract record AbstractEntityDto
{
    public abstract string Kind { get; }
    public string Eid { get; init; } = default!;
    public VectorDto Pos { get; init; } = default!;
}
