import produce from "immer"
import {
  createContext,
  ReactNode,
  useCallback,
  useContext,
  useDebugValue,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react"
import { WEBSOCKET_ROOT } from "../config"
import { FullRoomStateDto } from "./dtos"
import {
  RoomLocation,
  SocketEventData,
  SocketEventHandler,
  SocketEventTag,
  StateForRoom,
} from "./socketEvents"

interface SocketContextValue<R extends RoomLocation> {
  roomState: StateForRoom<R>
  sendMessage: <T extends SocketEventTag>(tag: T, data: SocketEventData<T>) => void
  useSocketEvent: <T extends SocketEventTag>(tag: T, handler: SocketEventHandler<T, R>) => void
}

const SocketContext = createContext({
  roomState: { location: "menu", lobbies: [] },
  sendMessage: (tag, data) => {},
  useSocketEvent: (tag, handler) => {},
} as SocketContextValue<RoomLocation>)

export function SocketContextProvider({ children }: { children: ReactNode }) {
  const wsRef = useRef<WebSocket>()
  const eventMapRef = useRef<
    Partial<{ [T in SocketEventTag]: SocketEventHandler<T, RoomLocation>[] }>
  >({})

  const [roomState, setRoomState] = useState<FullRoomStateDto>({
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

  const useSocketEvent = useMemo(
    () =>
      function useSocketEvent<T extends SocketEventTag>(
        tag: T,
        handler: SocketEventHandler<T, RoomLocation>
      ) {
        useEffect(() => {
          if (!(tag in eventMapRef.current)) {
            eventMapRef.current[tag] = []
          }
          eventMapRef.current[tag]!.push(handler)

          return () => {
            eventMapRef.current[tag] = eventMapRef.current[tag]!.filter(h => h !== handler) as any
          }
        }, [tag, handler])

        useDebugValue(tag)
      },
    []
  )

  const value = useMemo(
    () => ({ roomState, sendMessage, useSocketEvent }),
    [roomState, sendMessage, useSocketEvent]
  )

  return <SocketContext.Provider value={value}>{children}</SocketContext.Provider>
}

export function useSocketContext<R extends RoomLocation>() {
  return useContext(SocketContext) as SocketContextValue<R>
}
