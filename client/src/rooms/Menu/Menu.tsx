import { useSocketContext } from "../../utils/socketContext"
import { usePlayerName } from "../../utils/usePlayerName"

export function Menu() {
  const { roomState, sendMessage, useSocketEvent } = useSocketContext<"menu">()
  const playerName = usePlayerName()

  useSocketEvent("s-new-lobby", (data, draft) => {
    draft.lobbies.push(data)
  })

  useSocketEvent("s-lobby-removed", (data, draft) => {
    draft.lobbies = draft.lobbies.filter(({ lid }) => lid !== data)
  })

  return (
    <>
      <h5>Your name: {playerName}</h5>
      <button onClick={() => sendMessage("create-lobby", null)}>Create lobby</button>
      <h1>Menu</h1>
      <h2>Lobbies</h2>
      <ul>
        {roomState.lobbies.map(lobby => (
          <li key={lobby.lid}>
            {lobby.name} {lobby.players}{" "}
            <button onClick={() => sendMessage("enter-lobby", lobby.lid)}>Enter</button>
          </li>
        ))}
      </ul>
    </>
  )
}