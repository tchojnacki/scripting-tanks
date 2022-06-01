import {
  CustomColorsDto,
  FullRoomStateDto,
  InputAxesDto,
  LobbyDataDto,
  PlayerDataDto,
} from "./dtos"

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
  "c-customize-colors": CustomColorsDto
  "c-create-lobby": null
  "c-enter-lobby": string
  "c-leave-lobby": null
  "c-close-lobby": null
  "c-start-game": null
  "c-set-input-axes": InputAxesDto
  "c-set-barrel-target": number
  "c-shoot": null
}
