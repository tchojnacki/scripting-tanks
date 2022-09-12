namespace Backend.Domain.Identifiers;

public record EID : Identifier<EID>
{
    private EID() { }

    private EID(string source, bool encode) : base(source, encode) { }

    public static EID GenerateUnique() => new();

    public static EID FromCID(CID cid) => new(cid.ToString(), true);
}
