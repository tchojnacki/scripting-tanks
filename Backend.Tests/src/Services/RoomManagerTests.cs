using Backend.Contracts.Messages.Client;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;
using Backend.Domain.Rooms.GameStates;
using Backend.Mediation.Requests;
using Backend.Services;
using MediatR;

namespace Backend.Tests.Services;

public class RoomManagerTest
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly RoomManager _sut;

    public RoomManagerTest() => _sut = new(_mediator);

    private async Task<CID> SetupPlayerAsync()
    {
        var cid = CID.GenerateUnique();
        _mediator.Send(new PlayerDataRequest(cid)).Returns(new PlayerData { CID = cid });
        await _sut.HandleOnConnectAsync(cid);
        return cid;
    }

    private async Task<LID> SetupLobbyAsync(CID ownerCid)
    {
        await _sut.HandleOnMessageAsync(ownerCid, new CreateLobbyClientMessage());
        return ((GameRoom)_sut.RoomContaining(ownerCid)).LID;
    }

    [Fact]
    public void Lobbies_ShouldReturnEmptyList_WhenNoLobbiesArePresent()
    {
        var lobbies = _sut.Lobbies;

        lobbies.Should().BeEmpty();
    }

    [Fact]
    public async Task Lobbies_ShouldContainLobby_WhenLobbyWasCreated()
    {
        var cid = await SetupPlayerAsync();
        await SetupLobbyAsync(cid);

        var lobbies = _sut.Lobbies;

        lobbies.Should().ContainSingle();
    }

    [Fact]
    public async Task GetGameRoom_ShouldReturnGameRoom_WhenItExists()
    {
        var cid = await SetupPlayerAsync();
        var lid = await SetupLobbyAsync(cid);

        var room = _sut.GetGameRoom(lid);

        room.LID.Should().Be(lid);
    }

    [Fact]
    public async Task ContainsGameRoom_ShouldReturnTrue_WhenRoomExists()
    {
        var cid = await SetupPlayerAsync();
        var lid = await SetupLobbyAsync(cid);

        var result = _sut.ContainsGameRoom(lid);

        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsGameRoom_ShouldReturnFalse_WhenNoRoomWasFound()
    {
        var lid = LID.GenerateUnique();

        var result = _sut.ContainsGameRoom(lid);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task RoomContaining_ShouldReturnMenuRoom_WhenPlayerIsNotInLobby()
    {
        var cid = await SetupPlayerAsync();

        var room = _sut.RoomContaining(cid);

        room.Should().BeOfType<MenuRoom>();
    }

    [Fact]
    public async Task RoomContaining_ShouldReturnGameRoom_WhenPlayerIsInLobby()
    {
        var cid = await SetupPlayerAsync();
        await SetupLobbyAsync(cid);

        var room = _sut.RoomContaining(cid);

        room.Should().BeAssignableTo<GameRoom>()
            .Which.Should().Match<GameRoom>(gr => gr.HasPlayer(cid));
    }

    [Fact]
    public void MenuRoom_ShouldReturnValidMenuRoom()
    {
        var menu = _sut.MenuRoom;

        menu.Should().BeOfType<MenuRoom>();
    }

    [Fact]
    public async Task CloseLobbyAsync_RemovesLobbyFromList()
    {
        var cid = await SetupPlayerAsync();
        var lid = await SetupLobbyAsync(cid);

        await _sut.CloseLobbyAsync(lid);

        _sut.Lobbies.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleOnDisconnectAsync_ShouldRemovePlayerFromMenu()
    {
        var cid = await SetupPlayerAsync();
        Action act = () => _sut.RoomContaining(cid);

        await _sut.HandleOnDisconnectAsync(cid);

        act.Should().Throw<Exception>();
    }

    [Fact]
    public async Task HandleOnMessageAsync_ShouldDelegateMessage()
    {
        var cid = await SetupPlayerAsync();

        await _sut.HandleOnMessageAsync(cid, new CreateLobbyClientMessage());

        await _mediator.Received().Send(Arg.Any<BroadcastUpsertLobbyRequest>());
    }

    [Fact]
    public async Task JoinGameRoomAsync_ShouldChangePlayerLobby()
    {
        var ownerCid = await SetupPlayerAsync();
        var joinerCid = await SetupPlayerAsync();
        var lid = await SetupLobbyAsync(ownerCid);

        await _sut.JoinGameRoomAsync(joinerCid, lid);

        _sut.RoomContaining(joinerCid)
            .Should().BeAssignableTo<GameRoom>()
            .Which.LID.Should().Be(lid);
    }

    [Fact]
    public async Task StartGameAsync_ShouldChangeGameState_WhenInWaitingGameState()
    {
        var cid = await SetupPlayerAsync();
        await SetupLobbyAsync(cid);

        await _sut.HandleOnMessageAsync(cid, new StartGameClientMessage());

        _sut.RoomContaining(cid).Should().BeOfType<PlayingGameState>();
    }

    [Fact]
    public async Task ShowSummaryAsync_ShouldChangeGameState_WhenInPlayingGameState()
    {
        var cid = await SetupPlayerAsync();
        var lid = await SetupLobbyAsync(cid);
        await _sut.HandleOnMessageAsync(cid, new StartGameClientMessage());

        await _sut.ShowSummaryAsync(lid);

        _sut.GetGameRoom(lid).Should().BeOfType<SummaryGameState>();
    }

    [Fact]
    public async Task PlayAgainAsync_ShouldChangeGameState_WhenInSummaryGameState()
    {
        var cid = await SetupPlayerAsync();
        var lid = await SetupLobbyAsync(cid);
        await _sut.HandleOnMessageAsync(cid, new StartGameClientMessage());
        await _sut.ShowSummaryAsync(lid);

        await _sut.PlayAgainAsync(lid);

        _sut.GetGameRoom(lid).Should().BeOfType<WaitingGameState>();
    }

    [Fact]
    public async Task KickPlayerAsync_ShouldKickPlayer()
    {
        var ownerCid = await SetupPlayerAsync();
        var playerCid = await SetupPlayerAsync();
        var lid = await SetupLobbyAsync(ownerCid);
        await _sut.HandleOnMessageAsync(playerCid, new EnterLobbyClientMessage { Data = lid.ToString() });

        await _sut.HandleOnMessageAsync(ownerCid, new KickPlayerClientMessage { Data = playerCid.ToString() });

        _sut.GetGameRoom(lid).AllPlayers.Should().ContainSingle();
    }
}
