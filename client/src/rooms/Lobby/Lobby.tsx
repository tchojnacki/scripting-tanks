import { useSocketContext } from "../../utils/socketContext"
import { usePlayerName } from "../../utils/usePlayerName"

export function Lobby() {
  const { sendMessage, roomState, useSocketEvent } = useSocketContext<"lobby">()
  const playerName = usePlayerName()

  useSocketEvent("new-player", (data, draft) => {
    draft.players.push(data)
  })

  useSocketEvent("player-left", (data, draft) => {
    draft.players = draft.players.filter(p => p.cid !== data.cid)
  })

  useSocketEvent("owner-change", (data, draft) => {
    draft.owner = data.cid
  })

  return (
    <>
      <h5>Your name: {playerName}</h5>
      <button onClick={() => sendMessage("leave-lobby", null)}>Leave</button>
      <h1>{roomState.name}</h1>
      <h2>Players</h2>
      <ul>
        {roomState.players.map(player => (
          <li key={player.cid}>
            {player.name} {player.cid === roomState.owner && "👑"}
          </li>
        ))}
      </ul>
    </>
  )
}
