import { Menu, Lobby, Game, Summary } from "./rooms"
import { useSocketContext } from "./utils/socketContext"
import { SocketContextProvider } from "./utils/socketContext"
import { IdentityContextProvider } from "./utils/indentityContext"
import { ColorScheme, ColorSchemeProvider, MantineProvider } from "@mantine/core"
import { StrictMode, useState } from "react"

function Room() {
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

export function App() {
  const [colorScheme, setColorScheme] = useState<ColorScheme>("dark")
  const toggleColorScheme = () => setColorScheme(prev => (prev === "dark" ? "light" : "dark"))

  return (
    <StrictMode>
      <SocketContextProvider>
        <IdentityContextProvider>
          <ColorSchemeProvider colorScheme={colorScheme} toggleColorScheme={toggleColorScheme}>
            <MantineProvider theme={{ colorScheme }} withNormalizeCSS withGlobalStyles>
              <Room />
            </MantineProvider>
          </ColorSchemeProvider>
        </IdentityContextProvider>
      </SocketContextProvider>
    </StrictMode>
  )
}
