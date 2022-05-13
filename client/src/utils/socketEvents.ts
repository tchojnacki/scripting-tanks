import {
  AssignDisplayNameDto,
  NewLobbyDto,
  NewPlayerDto,
  OwnerChangeDto,
  PlayerLeftDto,
} from "./dtos"

interface MenuLobbyEntry {
  lid: string
  name: string
  players: number
}

interface LobbyPlayerEntry {
  cid: string
  name: string
}

type RoomStateMap = {
  lobby: {
    location: "lobby"
    name: string
    owner: string
    players: LobbyPlayerEntry[]
  }
  menu: {
    location: "menu"
    lobbies: MenuLobbyEntry[]
  }
}

export type RoomLocation = keyof RoomStateMap
export type StateForRoom<R extends RoomLocation> = RoomStateMap[R]
export type RoomState = StateForRoom<RoomLocation>

type EventDataMap = {
  "refetch-display-name": null
  "assign-display-name": AssignDisplayNameDto
  "new-player": NewPlayerDto
  "player-left": PlayerLeftDto
  "owner-change": OwnerChangeDto
  "full-room-state": RoomState
  "new-lobby": NewLobbyDto
  "create-lobby": null
  "enter-lobby": string
  "lobby-removed": { lid: string }
  "leave-lobby": null
}

export type SocketEventTag = keyof EventDataMap

export type SocketEventData<T extends SocketEventTag> = EventDataMap[T]

export type SocketEventHandler<T extends SocketEventTag, R extends RoomLocation> = (
  data: SocketEventData<T>,
  draft: StateForRoom<R>
) => void
