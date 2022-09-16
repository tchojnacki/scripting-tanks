using System.Text;
using System.Security.Cryptography;

namespace Backend.Domain.Identifiers;

internal abstract record Identifier<T> where T : Identifier<T>
{
    private const int DigestLength = 32;
    private static readonly string Prefix = typeof(T).Name + "$";

    private static string EncodeSource(string source)
    {
        var inputBytes = Encoding.ASCII.GetBytes(source);
        using var algorithm = MD5.Create();
        var hashedBytes = algorithm.ComputeHash(inputBytes);
        return Prefix + Convert.ToHexString(hashedBytes);
    }

    private readonly string _value;

    protected Identifier() : this(Guid.NewGuid().ToString()) { }

    protected Identifier(string source, bool encode = true)
    {
        var value = encode ? EncodeSource(source) : source;

        if (!value.StartsWith(Prefix) || value.Length != Prefix.Length + DigestLength)
        {
            if (encode)
                throw new InvalidOperationException("Value was not encoded successfully");
            else
                throw new ArgumentException("Value needs to be correctly encoded and prefixed", nameof(source));
        }

        _value = value;
    }

    public sealed override string ToString() => _value;
}
