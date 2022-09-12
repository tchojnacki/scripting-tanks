namespace Backend.Domain.Identifiers;

public record EID : Identifier<EID>
{
    private EID() { }

    private EID(string source) : base(source) { }

    public static EID GenerateUnique() => new();

    public static EID FromCID(CID cid) => new(cid.ToString());
}
