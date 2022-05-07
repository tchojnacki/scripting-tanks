import { useEffect, useRef, useState } from "react"
import { WEBSOCKET_ROOT } from "./config"
import { Menu, Lobby } from "./rooms"

export function App() {
  const wsRef = useRef<WebSocket>()

  const [roomState, setRoomState] = useState<any>({
    location: "menu",
    lobbies: [],
  })

  const [name, setName] = useState("")

  useEffect(() => {
    const ws = new WebSocket(`${WEBSOCKET_ROOT}/game`)

    ws.onmessage = event => {
      const message = JSON.parse(event.data)

      if (message.tag === "assign-display-name") {
        setName(message.data)
      } else if (message.tag === "full-room-state") {
        setRoomState(message.data)
      } else if (message.tag === "new-lobby") {
        setRoomState((prev: any) => ({ ...prev, lobbies: [...prev.lobbies, message.data] }))
      } else if (message.tag === "lobby-removed") {
        setRoomState((prev: any) => ({
          ...prev,
          lobbies: prev.lobbies.filter(({ lid }: any) => lid !== message.data.lid),
        }))
      } else if (message.tag === "new-player") {
        setRoomState((prev: any) => ({ ...prev, players: [...prev.players, message.data] }))
      } else if (message.tag === "owner-change") {
        setRoomState((prev: any) => ({ ...prev, owner: message.data }))
      } else if (message.tag === "player-left") {
        setRoomState((prev: any) => ({
          ...prev,
          players: prev.players.filter((p: any) => p.cid !== message.data),
        }))
      }
    }

    wsRef.current = ws

    return () => {
      ws.close()
    }
  }, [])

  function sendMessage(tag: string, data: any = null) {
    wsRef.current?.send(JSON.stringify({ tag, data }))
  }

  if (roomState.location === "menu") {
    return <Menu name={name} roomState={roomState} sendMessage={sendMessage} />
  } else if (roomState.location === "lobby") {
    return <Lobby name={name} roomState={roomState} sendMessage={sendMessage} />
  } else {
    return null
  }
}
