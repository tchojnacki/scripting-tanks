import { FullRoomStateDto } from "./dtos"
import { ClientMessageDataMap, ServerMessageDataMap } from "./messages"

type RoomStateMap = { [room in FullRoomStateDto as room["location"]]: room }
export type RoomLocation = keyof RoomStateMap
export type StateForRoom<R extends RoomLocation> = RoomStateMap[R]

export type SocketEventTag = keyof ServerMessageDataMap
export type SocketEventData<T extends SocketEventTag> = ServerMessageDataMap[T]
export type SocketEventHandler<T extends SocketEventTag, R extends RoomLocation> = (
  data: SocketEventData<T>,
  draft: StateForRoom<R>
) => void

export type SendMessageTag = keyof ClientMessageDataMap
export type SendMessageData<T extends SendMessageTag> = ClientMessageDataMap[T]
