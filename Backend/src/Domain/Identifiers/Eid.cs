namespace Backend.Domain.Identifiers;

internal sealed record Eid : Identifier<Eid>
{
    private Eid()
    {
    }

    private Eid(string source, bool encode) : base(source, encode)
    {
    }

    public static Eid GenerateUnique() => new();

    public static Eid FromCid(Cid cid) => new(cid.ToString(), true);
}
