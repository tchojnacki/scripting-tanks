import { useSocketContext } from "../../utils/socketContext"
import { usePlayerName } from "../../utils/usePlayerName"

export function Lobby() {
  const { sendMessage, roomState, useSocketEvent } = useSocketContext<"lobby">()
  const playerName = usePlayerName()

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
      <h5>Your name: {playerName}</h5>
      <button onClick={() => sendMessage("c-leave-lobby", null)}>Leave</button>
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
