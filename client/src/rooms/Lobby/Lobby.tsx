import { useSocketContext } from "../../utils/socketContext"
import { useIdentity } from "../../utils/indentityContext"
import { Button, Center, Group, Stack, Title, Tooltip } from "@mantine/core"
import { PlayerInfo, StandardLayout } from "../../components"
import { DoorExit, PlayerPlay, TrashX } from "tabler-icons-react"
import { useDocumentTitle } from "@mantine/hooks"

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
          <Title order={3}>Players</Title>
          <ul>
            {roomState.players.map(player => (
              <li key={player.cid} style={{ fontWeight: player.cid === cid ? "bold" : "normal" }}>
                {player.name} {player.cid === roomState.owner && "👑"}
              </li>
            ))}
          </ul>
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
                    leftIcon={<PlayerPlay size={16} />}
                    disabled={roomState.players.length < 2}
                    onClick={() => sendMessage("c-start-game", null)}
                  >
                    Start game
                  </Button>
                </Tooltip>
                <Button leftIcon={<TrashX size={16} />} color="red">
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
