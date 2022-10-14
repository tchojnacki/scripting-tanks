namespace Backend.Domain.Identifiers;

internal sealed record Lid : Identifier<Lid>
{
    private Lid()
    {
    }

    private Lid(string source, bool encode) : base(source, encode)
    {
    }

    public static Lid GenerateUnique() => new();

    public static Lid Deserialize(string source) => new(source, false);
}
