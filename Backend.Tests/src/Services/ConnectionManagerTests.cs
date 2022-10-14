using Backend.Contracts.Messages.Server;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Services;
using Microsoft.Extensions.Logging;

namespace Backend.Tests.Services;

public class ConnectionManagerTest
{
    private readonly ICustomizationProvider _customizationProvider = Substitute.For<ICustomizationProvider>();
    private readonly ILogger<ConnectionManager> _logger = Substitute.For<ILogger<ConnectionManager>>();
    private readonly IMessageSerializer _messageSerializer = Substitute.For<IMessageSerializer>();
    private readonly IMessageValidator _messageValidator = Substitute.For<IMessageValidator>();
    private readonly IRoomManager _roomManager = Substitute.For<IRoomManager>();
    private readonly ConnectionManager _sut;

    public ConnectionManagerTest() =>
        _sut = new(_roomManager, _customizationProvider, _messageSerializer, _messageValidator, _logger);

    private async Task<CID> SetupPlayerAsync()
    {
        var cid = CID.GenerateUnique();
        _customizationProvider.AssignDisplayName().Returns("Test name");
        _customizationProvider.AssignTankColors().Returns(new TankColors
        {
            TankColor = "#ffffff",
            TurretColor = "#ffffff"
        });
        await _sut.HandleOnConnectAsync(cid, default!);
        return cid;
    }

    [Fact]
    public async Task SendToSingleAsync_ShouldSerializeMessage_WhenPlayerExists()
    {
        var cid = await SetupPlayerAsync();
        var message = new LobbyRemovedServerMessage { Data = LID.GenerateUnique().ToString() };

        await _sut.SendToSingleAsync(cid, message);

        _messageSerializer.Received().Serialize(message);
    }

    [Fact]
    public async Task DataFor_ShouldReturnData_WhenPlayerExists()
    {
        var cid = await SetupPlayerAsync();

        var data = _sut.DataFor(cid);

        data.Name.Should().Be("Test name");
    }
}
