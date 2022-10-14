using System.Text;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Server;
using Backend.Services;

namespace Backend.Tests.Services;

public class MessageSerializerTest
{
    private readonly MessageSerializer _sut;

    public MessageSerializerTest() => _sut = new();

    [Theory]
    [InlineData("{ \"tag\": \"c-add-bot\" }")]
    [InlineData("{ \"tag\": \"c-add-bot\", \"data\": null }")]
    [InlineData("{ \"tag\": \"c-enter-lobby\", \"data\": \"LID$TEST\" }")]
    public void TryDeserialize_ShouldDeserialize_WhenMessageIsValid(string messageString)
    {
        var buffer = Encoding.UTF8.GetBytes(messageString);
        
        var result = _sut.TryDeserialize(buffer, out var message);

        result.Should().BeTrue();
        message.Should().BeAssignableTo<IClientMessage>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("{ \"tag\": \"invalid-tag\" }")]
    [InlineData("{ \"tag\": \"c-enter-lobby\", \"data\": 3 }")]
    public void TryDeserialize_ShouldReturnFalse_WhenMessageIsInvalid(string messageString)
    {
        var buffer = Encoding.UTF8.GetBytes(messageString);
        
        var result = _sut.TryDeserialize(buffer, out _);

        result.Should().BeFalse();
    }

    [Fact]
    public void Serialize_ShouldReturnJsonString_WhenServerMessageIsPassed()
    {
        var message = new LobbyRemovedServerMessage { Data = string.Empty };
        
        var bytes = _sut.Serialize(message);
        
        var text = Encoding.UTF8.GetString(bytes);
        text.Should().Contain("\"tag\":\"s-lobby-removed\"");
    }
}
