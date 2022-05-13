import { FullRoomStateDto, LobbyDataDto, PlayerDataDto } from "./dtos"

export type MessageDataMap = {
  "s-assign-display-name": string
  "s-new-player": PlayerDataDto
  "s-player-left": string
  "s-owner-change": string
  "s-full-room-state": FullRoomStateDto
  "s-new-lobby": LobbyDataDto
  "s-lobby-removed": string

  "refetch-display-name": null
  "create-lobby": null
  "enter-lobby": string
  "leave-lobby": null
}
