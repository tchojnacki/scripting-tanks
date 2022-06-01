import { useSocketContext } from "../../utils/socketContext"
import { useIdentity } from "../../utils/indentityContext"
import {
  Badge,
  Button,
  Group,
  Menu,
  Paper,
  SimpleGrid,
  Stack,
  Text,
  ThemeIcon,
  Title,
  Tooltip,
} from "@mantine/core"
import { PlayerInfo, StandardLayout } from "../../components"
import { Crown, DoorExit, PlayerPlay, Robot, TrashX, UserMinus } from "tabler-icons-react"
import { useDocumentTitle } from "@mantine/hooks"
import { sortBy } from "lodash"

export function Lobby() {
  const { sendMessage, roomState, useSocketEvent } = useSocketContext<"game-waiting">()
  const { cid } = useIdentity()
  const isOwner = roomState.owner === cid

  useSocketEvent("s-new-player", (data, draft) => {
    draft.players.push(data)
  })

  useSocketEvent("s-player-left", (data, draft) => {
    draft.players = draft.players.filter(p => p.cid !== data)
  })

  useSocketEvent("s-owner-change", (data, draft) => {
    draft.owner = data
  })

  useDocumentTitle(`${roomState.name} | Tanks`)

  return (
    <StandardLayout
      title={roomState.name}
      headerRight={
        roomState.players.length > 1 && (
          <Button
            color="orange"
            leftIcon={<DoorExit size={16} />}
            onClick={() => sendMessage("c-leave-lobby", null)}
          >
            Leave
          </Button>
        )
      }
      left={
        <>
          <Group>
            <Title order={3}>Players</Title>
            <Badge>{roomState.players.length}</Badge>
          </Group>
          <SimpleGrid py="xl" cols={2}>
            {sortBy(roomState.players, [p => (p.cid === roomState.owner ? 0 : 1), "name"]).map(
              player => (
                <Paper key={player.cid} py="xs" px="xl" radius="md" shadow="xl">
                  <Group position="apart">
                    <Text color={player.cid === cid ? "cyan" : "inherit"}>{player.name}</Text>
                    {player.cid === roomState.owner && (
                      <ThemeIcon variant="outline" color="yellow" size="sm" radius="xl">
                        <Crown size={16} />
                      </ThemeIcon>
                    )}
                    {isOwner && player.cid !== roomState.owner && (
                      <Menu trigger="hover" delay={100}>
                        <Menu.Label>Player</Menu.Label>
                        <Menu.Item
                          onClick={() => sendMessage("c-kick-player", player.cid)}
                          icon={<UserMinus size={16} />}
                          color="red"
                        >
                          Kick
                        </Menu.Item>
                        <Menu.Item
                          onClick={() => sendMessage("c-promote-player", player.cid)}
                          icon={<Crown size={16} />}
                          color="yellow"
                        >
                          Make owner
                        </Menu.Item>
                      </Menu>
                    )}
                  </Group>
                </Paper>
              )
            )}
          </SimpleGrid>
          {isOwner && (
            <Button disabled variant="light" leftIcon={<Robot size={16} />}>
              Add bot
            </Button>
          )}
        </>
      }
      right={
        <Stack spacing={64}>
          {isOwner && (
            <div>
              <Title order={3} pb="xl">
                Game settings
              </Title>
              <Group position="center">
                <Tooltip
                  opened={roomState.players.length < 2 ? undefined : false}
                  label="At least two players required."
                  position="bottom"
                  withArrow
                >
                  <Button
                    onClick={() => sendMessage("c-start-game", null)}
                    disabled={roomState.players.length < 2}
                    leftIcon={<PlayerPlay size={16} />}
                  >
                    Start game
                  </Button>
                </Tooltip>
                <Button
                  onClick={() => sendMessage("c-close-lobby", null)}
                  leftIcon={<TrashX size={16} />}
                  color="red"
                >
                  Close lobby
                </Button>
              </Group>
            </div>
          )}
          <PlayerInfo unmutable compact={isOwner} />
        </Stack>
      }
    />
  )
}
