using Backend.Services;
using MediatR;

namespace Backend.Tests.Services;

public class RoomManagerTest
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly RoomManager _sut;

    public RoomManagerTest() => _sut = new(_mediator);
}
