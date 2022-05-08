import produce from "immer"
import { createContext, ReactNode, useContext, useEffect, useRef, useState } from "react"
import { WEBSOCKET_ROOT } from "./config"

interface MenuLobbyEntry {
  lid: string
  name: string
  players: number
}

interface MenuRoomState {
  location: "menu"
  lobbies: MenuLobbyEntry[]
}

interface LobbyPlayerEntry {
  cid: string
  name: string
}

interface LobbyRoomState {
  location: "lobby"
  name: string
  owner: string
  players: LobbyPlayerEntry[]
}

type RoomState = MenuRoomState | LobbyRoomState

type SocketEventHandler = (data: unknown, draft: RoomState) => void

const SocketContext = createContext({
  roomState: { location: "menu", lobbies: [] } as RoomState,
  sendMessage: (tag: string, data: any = null) => {},
  useSocketEvent: (tag: string, handler: SocketEventHandler) => {},
})

export function SocketContextProvider({ children }: { children: ReactNode }) {
  const wsRef = useRef<WebSocket>()
  const eventMapRef = useRef<Record<string, SocketEventHandler[]>>({})

  const [roomState, setRoomState] = useState<RoomState>({
    location: "menu",
    lobbies: [],
  })

  useEffect(() => {
    wsRef.current = new WebSocket(WEBSOCKET_ROOT)

    wsRef.current.onmessage = event => {
      const { tag, data } = JSON.parse(event.data)
      if (eventMapRef.current[tag]) {
        eventMapRef.current[tag].forEach(handler =>
          setRoomState(produce(draft => handler(data, draft)))
        )
      }
    }

    return () => {
      wsRef.current?.close()
    }
  }, [])

  function sendMessage(tag: string, data: any = null) {
    wsRef.current?.send(JSON.stringify({ tag, data }))
  }

  function useSocketEvent(tag: string, handler: SocketEventHandler) {
    useEffect(() => {
      if (!eventMapRef.current[tag]) {
        eventMapRef.current[tag] = []
      }
      eventMapRef.current[tag].push(handler)

      return () => {
        eventMapRef.current[tag] = eventMapRef.current[tag]?.filter(h => h !== handler)
      }
    }, [tag, handler])
  }

  return (
    <SocketContext.Provider value={{ roomState, sendMessage, useSocketEvent }}>
      {children}
    </SocketContext.Provider>
  )
}

export function useSocketContext() {
  return useContext(SocketContext)
}
