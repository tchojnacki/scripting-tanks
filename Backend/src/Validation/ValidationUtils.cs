using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Domain.Rooms;
using Backend.Domain.Rooms.States;
using Backend.Domain.Identifiers;

namespace Backend.Validation;

public static class ValidationUtils
{
    public static IRuleBuilderOptions<T, CID> MustBePlayer<T>(
        this IRuleBuilder<T, CID> ruleBuilder,
        Func<PlayerData, bool> predicate,
        IConnectionManager connectionManager)
        => ruleBuilder.Must(cid => predicate(connectionManager.DataFor(cid)));

    public static IRuleBuilderOptions<T, CID> MustBeInMenu<T>(
        this IRuleBuilder<T, CID> ruleBuilder,
        IRoomManager roomManager)
        => ruleBuilder.Must(cid => roomManager.RoomContaining(cid) == roomManager.MenuRoom);

    public static IRuleBuilderOptions<T, CID> MustBeInGameRoom<T>(
        this IRuleBuilder<T, CID> ruleBuilder,
        Func<GameRoom, CID, T, bool> predicate,
        IRoomManager roomManager)
        => ruleBuilder.Must((t, cid) => roomManager.RoomContaining(cid) is GameRoom gr && predicate(gr, cid, t));

    public static IRuleBuilderOptions<T, CID> MustBeInGameRoom<T>(
        this IRuleBuilder<T, CID> ruleBuilder,
        IRoomManager roomManager)
        => ruleBuilder.MustBeInGameRoom((_, _, _) => true, roomManager);

    public static IRuleBuilderOptions<T, CID> MustBeRoomOwner<T>(
        this IRuleBuilder<T, CID> ruleBuilder,
        Func<GameRoom, bool> predicate,
        IRoomManager roomManager)
        => ruleBuilder.MustBeInGameRoom((gr, cid, _) => cid == gr.OwnerCID && predicate(gr), roomManager);

    public static IRuleBuilderOptions<T, LID> MustBeAValidGameRoom<T>(
        this IRuleBuilder<T, LID> ruleBuilder,
        IRoomManager roomManager)
        => ruleBuilder.Must(lid => roomManager.ContainsGameRoom(lid));

    public static IRuleBuilderOptions<T, CID> MustHaveAliveTank<T>(
        this IRuleBuilder<T, CID> ruleBuilder,
        IRoomManager roomManager)
        => ruleBuilder.Must(cid =>
            roomManager.RoomContaining(cid) is GameRoom gr &&
            gr.State is PlayingGameState pgs &&
            pgs.Tanks.Any(t => t.EID == EID.FromCID(cid)));
}
