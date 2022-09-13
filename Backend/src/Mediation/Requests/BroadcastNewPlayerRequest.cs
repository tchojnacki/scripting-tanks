using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public class BroadcastNewPlayerRequest : IRequest
{
    public LID LID { get; }
    public CID CID { get; }

    public BroadcastNewPlayerRequest(LID lid, CID cid)
    {
        LID = lid;
        CID = cid;
    }
}
