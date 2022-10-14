using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;
using Backend.Domain.Rooms.GameStates;
using Backend.Services;
using FluentValidation;

namespace Backend.Validation;

internal static class ValidationUtils
{
    public static IRuleBuilderOptions<T, Cid> MustBePlayer<T>(
        this IRuleBuilder<T, Cid> ruleBuilder,
        Func<PlayerData, bool> predicate,
        IConnectionManager connectionManager)
        => ruleBuilder.Must(cid => predicate(connectionManager.DataFor(cid)));

    public static IRuleBuilderOptions<T, Cid> MustBeInMenu<T>(
        this IRuleBuilder<T, Cid> ruleBuilder,
        IRoomManager roomManager)
        => ruleBuilder.Must(cid => roomManager.RoomContaining(cid) == roomManager.MenuRoom);

    public static IRuleBuilderOptions<T, Cid> MustBeInGameRoom<T>(
        this IRuleBuilder<T, Cid> ruleBuilder,
        Func<GameRoom, Cid, T, bool> predicate,
        IRoomManager roomManager)
        => ruleBuilder.Must((t, cid) => roomManager.RoomContaining(cid) is GameRoom gr && predicate(gr, cid, t));

    public static IRuleBuilderOptions<T, Cid> MustBeInGameRoom<T>(
        this IRuleBuilder<T, Cid> ruleBuilder,
        IRoomManager roomManager)
        => ruleBuilder.MustBeInGameRoom((_, _, _) => true, roomManager);

    public static IRuleBuilderOptions<T, Cid> MustBeRoomOwner<T>(
        this IRuleBuilder<T, Cid> ruleBuilder,
        Func<GameRoom, bool> predicate,
        IRoomManager roomManager)
        => ruleBuilder.MustBeInGameRoom((gr, cid, _) => cid == gr.OwnerCid && predicate(gr), roomManager);

    public static IRuleBuilderOptions<T, Lid> MustBeAValidGameRoom<T>(
        this IRuleBuilder<T, Lid> ruleBuilder,
        Func<GameRoom, bool> predicate,
        IRoomManager roomManager)
        => ruleBuilder.Must(lid => roomManager.ContainsGameRoom(lid) && predicate(roomManager.GetGameRoom(lid)));

    public static IRuleBuilderOptions<T, Lid> MustBeAValidGameRoom<T>(
        this IRuleBuilder<T, Lid> ruleBuilder,
        IRoomManager roomManager)
        => ruleBuilder.MustBeAValidGameRoom(_ => true, roomManager);

    public static IRuleBuilderOptions<T, Cid> MustHaveAliveTank<T>(
        this IRuleBuilder<T, Cid> ruleBuilder,
        IRoomManager roomManager)
        => ruleBuilder.Must(cid =>
            roomManager.RoomContaining(cid) is GameRoom gr &&
            gr is PlayingGameState pgs &&
            pgs.Tanks.Any(t => t.Eid == Eid.FromCid(cid)));
}