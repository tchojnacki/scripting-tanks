import { showNotification } from "@mantine/notifications"
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
import { WifiOff } from "tabler-icons-react"
import { WEBSOCKET_ROOT } from "../config"
import { FullRoomStateDto } from "./dtos"
import {
  RoomLocation,
  SendMessageData,
  SendMessageTag,
  SocketEventData,
  SocketEventHandler,
  SocketEventTag,
  StateForRoom,
} from "./socketEvents"

interface SocketContextValue<R extends RoomLocation> {
  roomState: StateForRoom<R>
  sendMessage: <T extends SendMessageTag>(tag: T, data: SendMessageData<T>) => void
  useSocketEvent: <T extends SocketEventTag>(tag: T, handler: SocketEventHandler<T, R>) => void
}

const SocketContext = createContext({
  roomState: { location: "menu", lobbies: [] },
  sendMessage: (tag, data) => {},
  useSocketEvent: (tag, handler) => {},
} as SocketContextValue<RoomLocation>)

const ws = new WebSocket(WEBSOCKET_ROOT)

export function SocketContextProvider({ children }: { children: ReactNode }) {
  const eventMapRef = useRef<
    Partial<{ [T in SocketEventTag]: SocketEventHandler<T, RoomLocation>[] }>
  >({})

  const [roomState, setRoomState] = useState<FullRoomStateDto>({
    location: "menu",
    lobbies: [],
  })

  useEffect(() => {
    ws.onmessage = event => {
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

    ws.onclose = () => {
      showNotification({
        disallowClose: true,
        autoClose: false,
        title: "Connection error",
        message: (
          <>
            You lost connection to the server!
            <br />
            Refresh the page to try again.
          </>
        ),
        color: "red",
        icon: <WifiOff />,
      })
    }
  }, [])

  const sendMessage = useCallback(<T extends SendMessageTag>(tag: T, data: SendMessageData<T>) => {
    const string = JSON.stringify({ tag, data })
    if (ws.readyState === WebSocket.OPEN) {
      ws.send(string)
    } else {
      ws.onopen = () => ws.send(string)
    }
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
