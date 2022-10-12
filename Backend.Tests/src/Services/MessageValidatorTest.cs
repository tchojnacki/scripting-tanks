using Microsoft.Extensions.Logging;
using FluentValidation;
using Backend.Contracts.Messages.Client;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;
using Backend.Domain.Rooms.GameStates;
using Backend.Services;
using Backend.Validation;
using MediatR;

namespace Backend.Tests.Services;

public class MessageValidatorTest
{
    private readonly MessageValidator _sut;
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();
    private readonly ILogger<MessageValidator> _logger = Substitute.For<ILogger<MessageValidator>>();

    public MessageValidatorTest() => _sut = new(_serviceProvider, _logger);

    [Fact]
    public void Validate_ShouldReturnTrue_WhenMessageIsCorrect()
    {
        var cid = CID.GenerateUnique();
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
        var cid = CID.GenerateUnique();
        var mediator = Substitute.For<IMediator>();
        var roomManager = Substitute.For<IRoomManager>();
        var menuRoom = new MenuRoom(mediator, roomManager);
        var gameRoom = WaitingGameState.CreateNew(mediator, cid, LID.GenerateUnique(), "Name");
        roomManager.MenuRoom.Returns(menuRoom);
        roomManager.RoomContaining(cid).Returns(gameRoom);
        var message = new CreateLobbyClientMessage();
        _serviceProvider
            .GetService(typeof(IValidator<MessageContext<CreateLobbyClientMessage>>))
            .Returns(new CreateLobbyValidator(roomManager));

        var result = _sut.Validate(cid, message);

        result.Should().BeFalse();
    }
}
