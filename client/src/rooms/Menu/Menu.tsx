import { useSocketContext } from "../../utils/socketContext"
import {
  ActionIcon,
  Button,
  Paper,
  Text,
  Stack,
  Title,
  useMantineColorScheme,
  Group,
  Badge,
  Avatar,
} from "@mantine/core"
import { PlayerInfo, StandardLayout } from "../../components"
import { CirclePlus, DoorEnter, Eye, MoonStars, Sun } from "tabler-icons-react"
import { useDocumentTitle } from "@mantine/hooks"
import { sortBy } from "lodash"
import { nameAbbr } from "../../utils/nameAbbr"

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
        <ActionIcon variant="default" onClick={() => toggleColorScheme()} size={32}>
          {colorScheme === "dark" ? <Sun /> : <MoonStars />}
        </ActionIcon>
      }
      left={
        <div>
          <Title order={3}>Lobbies</Title>
          <Stack py="xl">
            {sortBy(roomState.lobbies, [
              l => (l.location === "game-waiting" ? 0 : 1),
              l => -l.players,
              "name",
            ]).map(lobby => (
              <Paper key={lobby.lid} p="md" radius="md" shadow="xl" withBorder>
                <Group position="apart">
                  <Group>
                    <Avatar color="blue" radius="xl">
                      {nameAbbr(lobby.name)}
                    </Avatar>
                    <Text>{lobby.name}</Text>
                    <Badge>{lobby.players}</Badge>
                  </Group>
                  <Button
                    compact
                    color={lobby.location === "game-waiting" ? "blue" : "gray"}
                    leftIcon={
                      lobby.location === "game-waiting" ? (
                        <DoorEnter size={14} />
                      ) : (
                        <Eye size={14} />
                      )
                    }
                    onClick={() => sendMessage("c-enter-lobby", lobby.lid)}
                  >
                    {lobby.location === "game-waiting" ? "Enter" : "Spectate"}
                  </Button>
                </Group>
              </Paper>
            ))}
          </Stack>
          <Button
            leftIcon={<CirclePlus size={16} />}
            onClick={() => sendMessage("c-create-lobby", null)}
          >
            Create lobby
          </Button>
        </div>
      }
      right={<PlayerInfo />}
    />
  )
}
