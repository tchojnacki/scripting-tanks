namespace Backend.Domain.Identifiers;

internal sealed record Cid : Identifier<Cid>
{
    private Cid()
    {
    }

    private Cid(string source, bool encode) : base(source, encode)
    {
    }

    public static Cid GenerateUnique() => new();

    public static Cid FromConnection(ConnectionInfo connectionInfo) => new(connectionInfo.Id, true);

    public static Cid Deserialize(string source) => new(source, false);
}
