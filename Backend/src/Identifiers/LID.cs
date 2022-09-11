using ValueOf;

namespace Backend.Identifiers;

public class LID : ValueOf<string, LID>
{
    protected override void Validate()
    {
        if (!Value.StartsWith("LID$"))
            throw new ArgumentException(Value);
    }
}
