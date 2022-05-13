import { FullRoomStateDto, LobbyDataDto, PlayerDataDto } from "./dtos"

export type ServerMessageDataMap = {
  "s-assign-display-name": string
  "s-new-player": PlayerDataDto
  "s-player-left": string
  "s-owner-change": string
  "s-full-room-state": FullRoomStateDto
  "s-new-lobby": LobbyDataDto
  "s-lobby-removed": string
}

export type ClientMessageDataMap = {
  "c-refetch-display-name": null
  "c-create-lobby": null
  "c-enter-lobby": string
  "c-leave-lobby": null
}
