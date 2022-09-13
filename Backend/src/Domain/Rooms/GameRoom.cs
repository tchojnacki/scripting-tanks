using MediatR;
using Backend.Services;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms.States;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;

namespace Backend.Domain.Rooms;

public class GameRoom : ConnectionRoom
{
    private static readonly Random Rand = new();

    private GameState _gameState;

    public GameRoom(
        IMediator mediator,
        CID owner,
        LID lid,
        string name)
        : base(mediator)
    {
        LID = lid;
        OwnerCID = owner;
        Name = name;
        _gameState = new WaitingGameState(mediator, this);
    }

    public LID LID { get; }
    public string Name { get; }
    public CID OwnerCID { get; private set; }

    public string Location => _gameState.RoomState.Location;

    public override AbstractRoomStateDto RoomState => _gameState.RoomState;

    private async Task SwitchStateAsync<TFrom>(GameState newState)
        where TFrom : GameState
    {
        if (_gameState is TFrom)
        {
            _gameState = newState;
            await _mediator.Send(new BroadcastRoomStateRequest(LID));
            await _mediator.Send(new BroadcastUpsertLobbyRequest(LID));
        }
    }

    public Task StartGameAsync() => SwitchStateAsync<WaitingGameState>(new PlayingGameState(_mediator, this));

    public Task ShowSummary(IReadOnlyScoreboard scoreboard)
        => SwitchStateAsync<PlayingGameState>(new SummaryGameState(_mediator, this, scoreboard));

    public Task PlayAgain() => SwitchStateAsync<SummaryGameState>(new WaitingGameState(_mediator, this));

    public async Task PromoteAsync(CID cid)
    {
        var data = await _mediator.Send(new PlayerDataRequest(cid));
        if (HasPlayer(cid) && !data.IsBot)
        {
            OwnerCID = cid;
            await _mediator.Send(new BroadcastOwnerChangeRequest(LID, OwnerCID));
        }
    }

    public override async Task HandleOnJoinAsync(CID cid)
    {
        await _gameState.HandleOnJoinAsync(cid);
        await base.HandleOnJoinAsync(cid);
        await _mediator.Send(new BroadcastUpsertLobbyRequest(LID));
    }

    public override async Task HandleOnLeaveAsync(CID cid)
    {
        await base.HandleOnLeaveAsync(cid);
        await _gameState.HandleOnLeaveAsync(cid);
        if (cid == OwnerCID)
        {
            var ownerCandidates = RealPlayers.Select(p => p.CID).ToList();
            if (ownerCandidates.Any())
            {
                OwnerCID = ownerCandidates.ElementAt(Rand.Next(ownerCandidates.Count));
                await _mediator.Send(new BroadcastOwnerChangeRequest(LID, OwnerCID));
            }
            else
            {
                await _mediator.Send(new CloseLobbyRequest(LID));
            }
        }

        if (RealPlayers.Any())
            await _mediator.Send(new BroadcastUpsertLobbyRequest(LID));
    }

    public override Task HandleOnMessageAsync(CID cid, IClientMessage message)
        => _gameState.HandleOnMessageAsync(cid, message);
}
