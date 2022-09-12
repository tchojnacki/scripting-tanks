namespace Backend.Domain.Identifiers;

public record LID : Identifier<LID>
{
    private LID() { }

    private LID(string source, bool encode) : base(source, encode) { }

    public static LID GenerateUnique() => new();

    public static LID Deserialize(string source) => new(source, false);
}
