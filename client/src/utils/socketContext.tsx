import produce from "immer"
import {
  createContext,
  ReactNode,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react"
import { WEBSOCKET_ROOT } from "../config"
import { RoomState, SocketEventData, SocketEventHandler, SocketEventTag } from "./socketEvents"

const SocketContext = createContext({
  roomState: { location: "menu", lobbies: [] } as RoomState,
  sendMessage: <T extends SocketEventTag>(tag: T, data: SocketEventData<T>) => {},
  useSocketEvent: <T extends SocketEventTag>(tag: T, handler: SocketEventHandler<T>) => {},
})

export function SocketContextProvider({ children }: { children: ReactNode }) {
  const wsRef = useRef<WebSocket>()
  const eventMapRef = useRef<Partial<{ [T in SocketEventTag]: SocketEventHandler<T>[] }>>({})

  const [roomState, setRoomState] = useState<RoomState>({
    location: "menu",
    lobbies: [],
  })

  useEffect(() => {
    wsRef.current = new WebSocket(WEBSOCKET_ROOT)

    wsRef.current.onmessage = event => {
      const { tag, data } = JSON.parse(event.data) as {
        tag: SocketEventTag
        data: SocketEventData<SocketEventTag>
      }

      if (tag in eventMapRef.current) {
        eventMapRef.current[tag]?.forEach(handler =>
          setRoomState(produce(draft => handler(data as never, draft)))
        )
      }
    }

    return () => {
      wsRef.current?.close()
    }
  }, [])

  const sendMessage = useCallback(<T extends SocketEventTag>(tag: T, data: SocketEventData<T>) => {
    wsRef.current?.send(JSON.stringify({ tag, data }))
  }, [])

  function useSocketEvent<T extends SocketEventTag>(tag: T, handler: SocketEventHandler<T>) {
    useEffect(() => {
      if (!(tag in eventMapRef.current)) {
        eventMapRef.current[tag] = []
      }
      eventMapRef.current[tag]!.push(handler)

      return () => {
        eventMapRef.current[tag] = eventMapRef.current[tag]!.filter(h => h !== handler) as any
      }
    }, [tag, handler])
  }

  const value = useMemo(
    () => ({ roomState, sendMessage, useSocketEvent }),
    [roomState, sendMessage, useSocketEvent]
  )

  return <SocketContext.Provider value={value}>{children}</SocketContext.Provider>
}

export function useSocketContext() {
  return useContext(SocketContext)
}
