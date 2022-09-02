import { FullRoomStateDto, InputAxesDto, LobbyDataDto, PlayerDataDto, TankColorsDto } from "./dtos"

export type ServerMessageDataMap = {
  "s-assign-identity": PlayerDataDto
  "s-new-player": PlayerDataDto
  "s-player-left": string
  "s-owner-change": string
  "s-full-room-state": FullRoomStateDto
  "s-upsert-lobby": LobbyDataDto
  "s-lobby-removed": string
}

export type ClientMessageDataMap = {
  "c-reroll-name": null
  "c-customize-colors": TankColorsDto
  "c-create-lobby": null
  "c-enter-lobby": string
  "c-leave-lobby": null
  "c-close-lobby": null
  "c-add-bot": null
  "c-promote-player": string
  "c-kick-player": string
  "c-start-game": null
  "c-set-input-axes": InputAxesDto
  "c-set-barrel-target": number
  "c-shoot": null
}
