import { Menu, Lobby } from "./rooms"
import { useSocketContext } from "./socketContext"

export function App() {
  const { roomState, useSocketEvent } = useSocketContext()

  useSocketEvent("full-room-state", data => data)

  if (roomState.location === "menu") {
    return <Menu />
  } else if (roomState.location === "lobby") {
    return <Lobby />
  } else {
    return null
  }
}
