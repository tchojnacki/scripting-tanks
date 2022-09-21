import { Menu, Lobby, Game, Summary } from "./rooms"
import { useSocketContext } from "./utils/socketContext"
import { SocketContextProvider } from "./utils/socketContext"
import { IdentityContextProvider } from "./utils/indentityContext"
import { ColorScheme, ColorSchemeProvider, MantineProvider } from "@mantine/core"
import { StrictMode, useState } from "react"
import { NotificationsProvider } from "@mantine/notifications"

function Room() {
  const { roomState, useSocketEvent } = useSocketContext()

  useSocketEvent("s-full-room-state", data => data)

  switch (roomState.location) {
    case "menu":
      return <Menu />
    case "game-waiting":
      return <Lobby />
    case "game-playing":
      return <Game />
    case "game-summary":
      return <Summary />
    default:
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
              <NotificationsProvider position="top-center">
                <Room />
              </NotificationsProvider>
            </MantineProvider>
          </ColorSchemeProvider>
        </IdentityContextProvider>
      </SocketContextProvider>
    </StrictMode>
  )
}
