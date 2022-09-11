using ValueOf;

namespace Backend.Identifiers;

public class CID : ValueOf<string, CID>
{
    protected override void Validate()
    {
        if (!Value.StartsWith("CID$"))
            throw new ArgumentException(Value);
    }
}
