using Backend.Contracts.Messages.Client;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;
using Backend.Domain.Rooms.GameStates;
using Backend.Services;
using Backend.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Tests.Services;

public class MessageValidatorTest
{
    private readonly ILogger<MessageValidator> _logger = Substitute.For<ILogger<MessageValidator>>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();
    private readonly MessageValidator _sut;

    public MessageValidatorTest() => _sut = new(_serviceProvider, _logger);

    [Fact]
    public void Validate_ShouldReturnTrue_WhenMessageIsCorrect()
    {
        var cid = Cid.GenerateUnique();
        var roomManager = Substitute.For<IRoomManager>();
        var menuRoom = new MenuRoom(Substitute.For<IMediator>(), roomManager);
        roomManager.MenuRoom.Returns(menuRoom);
        roomManager.RoomContaining(cid).Returns(menuRoom);
        var message = new RerollNameClientMessage();
        _serviceProvider
            .GetService(typeof(IValidator<MessageContext<RerollNameClientMessage>>))
            .Returns(new RerollNameValidator(roomManager));

        var result = _sut.Validate(cid, message);

        result.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenMessageIsIncorrect()
    {
        var cid = Cid.GenerateUnique();
        var mediator = Substitute.For<IMediator>();
        var roomManager = Substitute.For<IRoomManager>();
        var menuRoom = new MenuRoom(mediator, roomManager);
        var gameRoom = WaitingGameState.CreateNew(mediator, cid, Lid.GenerateUnique(), "Name");
        roomManager.MenuRoom.Returns(menuRoom);
        roomManager.RoomContaining(cid).Returns(gameRoom);
        var message = new CreateLobbyClientMessage();
        _serviceProvider
            .GetService(typeof(IValidator<MessageContext<CreateLobbyClientMessage>>))
            .Returns(new CreateLobbyValidator(roomManager));

        var result = _sut.Validate(cid, message);

        result.Should().BeFalse();
    }

    [Fact]
    public void Validate_ShouldLogWarning_WhenNoValidatorExists()
    {
        _sut.Validate(Cid.GenerateUnique(), new ShootClientMessage());

        _logger.ReceivedWithAnyArgs().LogWarning(default);
    }
}