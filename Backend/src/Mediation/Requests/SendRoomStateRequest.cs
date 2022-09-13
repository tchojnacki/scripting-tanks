using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public class SendRoomStateRequest : IRequest
{
    public CID CID { get; }
    public LID? LID { get; }

    public SendRoomStateRequest(CID cid, LID? lid)
    {
        CID = cid;
        LID = lid;
    }
}
