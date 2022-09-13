using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public class BroadcastPlayerLeftRequest : IRequest
{
    public LID LID { get; }
    public CID CID { get; }

    public BroadcastPlayerLeftRequest(LID lid, CID cid)
    {
        LID = lid;
        CID = cid;
    }
}
