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
        State = new WaitingGameState(mediator, this);
    }

    public LID LID { get; }
    public string Name { get; }
    public CID OwnerCID { get; private set; }
    public GameState State { get; private set; }

    public string Location => State.RoomState.Location;

    public override AbstractRoomStateDto RoomState => State.RoomState;

    private async Task SwitchStateAsync<TFrom>(GameState newState)
        where TFrom : GameState
    {
        if (State is TFrom)
        {
            State = newState;
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
        OwnerCID = cid;
        await _mediator.Send(new BroadcastOwnerChangeRequest(LID, OwnerCID));
    }

    public override async Task HandleOnJoinAsync(CID cid)
    {
        await State.HandleOnJoinAsync(cid);
        await base.HandleOnJoinAsync(cid);
        await _mediator.Send(new BroadcastUpsertLobbyRequest(LID));
    }

    public override async Task HandleOnLeaveAsync(CID cid)
    {
        await base.HandleOnLeaveAsync(cid);
        await State.HandleOnLeaveAsync(cid);
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
        => State.HandleOnMessageAsync(cid, message);
}
