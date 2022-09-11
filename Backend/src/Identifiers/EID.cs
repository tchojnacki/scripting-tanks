using ValueOf;

namespace Backend.Identifiers;

public class EID : ValueOf<string, EID>
{
    protected override void Validate()
    {
        if (!Value.StartsWith("EID$"))
            throw new ArgumentException(Value);
    }
}
