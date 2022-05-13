import { FullRoomStateDto } from "./dtos"
import { MessageDataMap } from "./messages"

type RoomStateMap = { [room in FullRoomStateDto as room["location"]]: room }
export type RoomLocation = keyof RoomStateMap
export type StateForRoom<R extends RoomLocation> = RoomStateMap[R]

export type SocketEventTag = keyof MessageDataMap
export type SocketEventData<T extends SocketEventTag> = MessageDataMap[T]
export type SocketEventHandler<T extends SocketEventTag, R extends RoomLocation> = (
  data: SocketEventData<T>,
  draft: StateForRoom<R>
) => void
