import { useSocketContext } from "../../utils/socketContext"
import { useIdentity } from "../../utils/indentityContext"

export function Lobby() {
  const { sendMessage, roomState, useSocketEvent } = useSocketContext<"game-waiting">()
  const { name, cid } = useIdentity()
  const isOwner = roomState.owner === cid

  useSocketEvent("s-new-player", (data, draft) => {
    draft.players.push(data)
  })

  useSocketEvent("s-player-left", (data, draft) => {
    draft.players = draft.players.filter(p => p.cid !== data)
  })

  useSocketEvent("s-owner-change", (data, draft) => {
    draft.owner = data
  })

  return (
    <>
      <h5>Your name: {name}</h5>
      {isOwner && (
        <button
          disabled={roomState.players.length < 2}
          onClick={() => sendMessage("c-start-game", null)}
        >
          Start game
        </button>
      )}
      <button onClick={() => sendMessage("c-leave-lobby", null)}>Leave</button>
      <h1>{roomState.name}</h1>
      <h2>Players</h2>
      <ul>
        {roomState.players.map(player => (
          <li key={player.cid} style={{ fontWeight: player.cid === cid ? "bold" : "normal" }}>
            {player.name} {player.cid === roomState.owner && "👑"}
          </li>
        ))}
      </ul>
    </>
  )
}
