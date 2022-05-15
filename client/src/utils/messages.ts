import { EntityDataDto, FullRoomStateDto, InputAxesDto, LobbyDataDto, PlayerDataDto } from "./dtos"

export type ServerMessageDataMap = {
  "s-assign-identity": PlayerDataDto
  "s-new-player": PlayerDataDto
  "s-player-left": string
  "s-owner-change": string
  "s-full-room-state": FullRoomStateDto
  "s-upsert-lobby": LobbyDataDto
  "s-lobby-removed": string
  "s-entity-update": EntityDataDto
}

export type ClientMessageDataMap = {
  "c-create-lobby": null
  "c-enter-lobby": string
  "c-leave-lobby": null
  "c-start-game": null
  "c-set-input-axes": InputAxesDto
}
