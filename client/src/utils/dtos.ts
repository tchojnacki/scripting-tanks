export interface LobbyDataDto {
  lid: string
  name: string
  players: number
}

export interface PlayerDataDto {
  cid: string
  name: string
}

export interface FullGameStateDto {
  location: "lobby"
  name: string
  owner: string
  players: PlayerDataDto[]
}

export interface FullMenuStateDto {
  location: "menu"
  lobbies: LobbyDataDto[]
}

export type FullRoomStateDto = FullGameStateDto | FullMenuStateDto
