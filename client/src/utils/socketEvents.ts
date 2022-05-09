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

export type RoomState = MenuRoomState | LobbyRoomState

type EventDataMap = {
  "refetch-display-name": null
  "assign-display-name": string
  "new-player": { cid: string; name: string }
  "player-left": string
  "owner-change": string
  "full-room-state": RoomState
  "new-lobby": { lid: string; name: string; players: number }
  "create-lobby": null
  "enter-lobby": string
  "lobby-removed": { lid: string }
  "leave-lobby": null
}

export type SocketEventTag = keyof EventDataMap

export type SocketEventData<T extends SocketEventTag> = EventDataMap[T]

export type SocketEventHandler<T extends SocketEventTag> = (
  data: SocketEventData<T>,
  draft: RoomState
) => void
