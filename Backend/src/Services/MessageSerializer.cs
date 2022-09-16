using System.Text;
using System.Text.Json;
using Backend.Contracts.Messages;

namespace Backend.Services;

internal sealed class MessageSerializer : IMessageSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IReadOnlyDictionary<string, Type> _tagMap;

    public MessageSerializer()
    {
        var messageTypes = typeof(MessageSerializer).Assembly.ExportedTypes
            .Where(
                t => t.IsAssignableTo(typeof(IClientMessage)) && !t.IsAbstract && !t.IsInterface
            );

        _tagMap = messageTypes.ToDictionary(t => (string)((dynamic)Activator.CreateInstance(t)!).Tag);
    }

    public bool TryDeserialize(byte[] buffer, out IClientMessage message)
    {
        message = default!;
        try
        {
            var content = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
            var tag = JsonDocument.Parse(content).RootElement.GetProperty("tag").GetString()!;
            var type = _tagMap[tag]!;

            message = (IClientMessage)JsonSerializer.Deserialize(content, type, SerializerOptions)!;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public byte[] Serialize<T>(IServerMessage<T> message)
        => JsonSerializer.SerializeToUtf8Bytes(message, SerializerOptions);
}
