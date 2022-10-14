using Backend.Contracts.Messages.Client;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;
using Backend.Domain.Rooms.GameStates;
using MediatR;

namespace Backend.Tests.Domain;

public class GameRoomTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly Cid _ownerCid = Cid.GenerateUnique();
    private readonly GameRoom _sut;

    public GameRoomTests() =>
        _sut = WaitingGameState.CreateNew(_mediator, _ownerCid, Lid.GenerateUnique(), "Room name");

    [Fact]
    public async Task PromoteAsync_ShouldChangeLobbyOwner()
    {
        var newOwner = Cid.GenerateUnique();

        await _sut.HandleOnMessageAsync(_ownerCid, new PromotePlayerClientMessage { Data = newOwner.ToString() });

        _sut.OwnerCid.Should().Be(newOwner);
    }
}