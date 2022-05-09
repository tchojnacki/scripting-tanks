import { useSocketContext } from "../../utils/socketContext"
import { usePlayerName } from "../../utils/usePlayerName"

export function Menu() {
  const { roomState, sendMessage, useSocketEvent } = useSocketContext()
  const playerName = usePlayerName()

  useSocketEvent("new-lobby", (data, draft) => {
    if (draft.location === "menu") {
      draft.lobbies.push(data)
    }
  })

  useSocketEvent("lobby-removed", (data, draft) => {
    if (draft.location === "menu") {
      draft.lobbies = draft.lobbies.filter(({ lid }) => lid !== data.lid)
    }
  })

  if (roomState.location !== "menu") return null

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
