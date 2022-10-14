using Backend.Contracts.Messages.Server;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms.GameStates;
using Backend.Services;
using MediatR;

namespace Backend.Tests.Services;

public class BroadcastHelperTest
{
    private readonly IConnectionManager _connectionManager = Substitute.For<IConnectionManager>();
    private readonly IRoomManager _roomManager = Substitute.For<IRoomManager>();
    private readonly BroadcastHelper _sut;

    public BroadcastHelperTest() => _sut = new(_connectionManager, _roomManager);

    [Fact]
    public async Task BroadcastToRoom_ShouldNotSendMessages_WhenLobbyIsEmpty()
    {
        var mediator = Substitute.For<IMediator>();
        var cid = Cid.GenerateUnique();
        var lid = Lid.GenerateUnique();
        var room = WaitingGameState.CreateNew(mediator, cid, lid, "Room name");
        var message = new LobbyRemovedServerMessage { Data = lid.ToString() };
        _roomManager.GetGameRoom(lid).Returns(room);

        await _sut.BroadcastToRoom(lid, message);

        await _connectionManager.DidNotReceive().SendToSingleAsync(cid, message);
    }
}