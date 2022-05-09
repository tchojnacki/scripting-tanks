import { useSocketContext } from "../../utils/socketContext"
import { usePlayerName } from "../../utils/usePlayerName"

export function Lobby() {
  const { sendMessage, roomState, useSocketEvent } = useSocketContext()
  const playerName = usePlayerName()

  useSocketEvent("new-player", (data, draft) => {
    if (draft.location === "lobby") {
      draft.players.push(data)
    }
  })

  useSocketEvent("player-left", (data, draft) => {
    if (draft.location === "lobby") {
      draft.players = draft.players.filter(p => p.cid !== data)
    }
  })

  useSocketEvent("owner-change", (data, draft) => {
    if (draft.location === "lobby") {
      draft.owner = data
    }
  })

  if (roomState.location !== "lobby") return null

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
