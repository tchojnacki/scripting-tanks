import { Menu, Lobby, Game, Summary } from "./rooms"
import { useSocketContext } from "./utils/socketContext"

export function App() {
  const { roomState, useSocketEvent } = useSocketContext()

  useSocketEvent("s-full-room-state", data => data)

  if (roomState.location === "menu") {
    return <Menu />
  } else if (roomState.location === "game-waiting") {
    return <Lobby />
  } else if (roomState.location === "game-playing") {
    return <Game />
  } else if (roomState.location === "game-summary") {
    return <Summary />
  } else {
    return null
  }
}
