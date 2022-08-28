namespace Backend.Contracts.Messages.Client;

public static class ClientMessageFactory
{
    private static readonly IReadOnlyDictionary<string, Type> TagMap;

    static ClientMessageFactory()
    {
        var messageTypes = typeof(ClientMessageFactory).Assembly.ExportedTypes
            .Where(
                t => t.GetInterfaces().Any(
                    i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IClientMessage<>))
                ) && !t.IsAbstract && !t.IsInterface
            );

        TagMap = messageTypes.ToDictionary(t => (string)((dynamic)Activator.CreateInstance(t)!).Tag);
    }

    public static Type? TagToType(string tag) => TagMap[tag];
}
