using MediatR;
using Backend.Domain;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public class PlayerDataRequest : IRequest<PlayerData>
{
    public CID CID { get; }

    public PlayerDataRequest(CID cid) => CID = cid;
}
