import { useSocketContext } from "../../utils/socketContext"
import { useIdentity } from "../../utils/indentityContext"

export function Menu() {
  const { roomState, sendMessage, useSocketEvent } = useSocketContext<"menu">()
  const { name } = useIdentity()

  useSocketEvent("s-upsert-lobby", (data, draft) => {
    draft.lobbies = draft.lobbies.filter(({ lid }) => lid !== data.lid)
    draft.lobbies.push(data)
  })

  useSocketEvent("s-lobby-removed", (data, draft) => {
    console.log(data)
    draft.lobbies = draft.lobbies.filter(({ lid }) => lid !== data)
  })

  return (
    <>
      <h5>Your name: {name}</h5>
      <button onClick={() => sendMessage("c-create-lobby", null)}>Create lobby</button>
      <h1>Menu</h1>
      <h2>Lobbies</h2>
      <ul>
        {roomState.lobbies.map(lobby => (
          <li key={lobby.lid}>
            {lobby.name} {lobby.players}{" "}
            <button onClick={() => sendMessage("c-enter-lobby", lobby.lid)}>Enter</button>
          </li>
        ))}
      </ul>
    </>
  )
}
