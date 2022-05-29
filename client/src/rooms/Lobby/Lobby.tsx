import { useSocketContext } from "../../utils/socketContext"
import { useIdentity } from "../../utils/indentityContext"
import { Button } from "@mantine/core"
import { PlayerInfo, StandardLayout } from "../../components"
import { DoorExit } from "tabler-icons-react"
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
        <Button
          color={roomState.players.length === 1 ? "red" : "orange"}
          leftIcon={<DoorExit size={16} />}
          onClick={() => sendMessage("c-leave-lobby", null)}
        >
          {roomState.players.length === 1 ? "Close" : "Leave"}
        </Button>
      }
    >
      <div>
        {isOwner && (
          <button
            disabled={roomState.players.length < 2}
            onClick={() => sendMessage("c-start-game", null)}
          >
            Start game
          </button>
        )}
        <h2>Players</h2>
        <ul>
          {roomState.players.map(player => (
            <li key={player.cid} style={{ fontWeight: player.cid === cid ? "bold" : "normal" }}>
              {player.name} {player.cid === roomState.owner && "👑"}
            </li>
          ))}
        </ul>
      </div>
      <PlayerInfo />
    </StandardLayout>
  )
}
