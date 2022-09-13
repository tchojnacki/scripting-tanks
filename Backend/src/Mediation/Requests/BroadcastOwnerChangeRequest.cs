using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public class BroadcastOwnerChangeRequest : IRequest
{
    public LID LID { get; }
    public CID NewOwnerCID { get; }

    public BroadcastOwnerChangeRequest(LID lid, CID newOwnerCid)
    {
        LID = lid;
        NewOwnerCID = newOwnerCid;
    }
}
