import { useSocketContext } from "../../utils/socketContext"
import { ActionIcon, Button, Title, useMantineColorScheme } from "@mantine/core"
import { PlayerInfo, StandardLayout } from "../../components"
import { CirclePlus, MoonStars, Sun } from "tabler-icons-react"
import { useDocumentTitle } from "@mantine/hooks"

export function Menu() {
  useDocumentTitle("Menu | Tanks")

  const { colorScheme, toggleColorScheme } = useMantineColorScheme()
  const { roomState, sendMessage, useSocketEvent } = useSocketContext<"menu">()

  useSocketEvent("s-upsert-lobby", (data, draft) => {
    draft.lobbies = draft.lobbies.filter(({ lid }) => lid !== data.lid)
    draft.lobbies.push(data)
  })

  useSocketEvent("s-lobby-removed", (data, draft) => {
    draft.lobbies = draft.lobbies.filter(({ lid }) => lid !== data)
  })

  return (
    <StandardLayout
      title="Menu"
      headerRight={
        <ActionIcon variant="default" onClick={() => toggleColorScheme()} size={30}>
          {colorScheme === "dark" ? <Sun size={16} /> : <MoonStars size={16} />}
        </ActionIcon>
      }
    >
      <div>
        <Title order={3}>Lobbies</Title>
        <ul>
          {roomState.lobbies.map(lobby => (
            <li key={lobby.lid}>
              {lobby.name} {lobby.players}{" "}
              <button onClick={() => sendMessage("c-enter-lobby", lobby.lid)}>
                {lobby.location === "game-playing" ? "Spectate" : "Enter"}
              </button>
            </li>
          ))}
        </ul>
        <Button
          leftIcon={<CirclePlus size={16} />}
          onClick={() => sendMessage("c-create-lobby", null)}
        >
          Create lobby
        </Button>
      </div>
      <PlayerInfo />
    </StandardLayout>
  )
}
