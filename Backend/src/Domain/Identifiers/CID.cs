namespace Backend.Domain.Identifiers;

internal sealed record CID : Identifier<CID>
{
    private CID() { }

    private CID(string source, bool encode) : base(source, encode) { }

    public static CID GenerateUnique() => new();

    public static CID FromConnection(ConnectionInfo connectionInfo) => new(connectionInfo.Id, true);

    public static CID Deserialize(string source) => new(source, false);
}
