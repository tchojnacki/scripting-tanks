import { Menu, Lobby } from "./rooms"
import { useSocketContext } from "./utils/socketContext"

export function App() {
  const { roomState, useSocketEvent } = useSocketContext()

  useSocketEvent("s-full-room-state", data => data)

  if (roomState.location === "menu") {
    return <Menu />
  } else if (roomState.location === "lobby") {
    return <Lobby />
  } else {
    return null
  }
}
