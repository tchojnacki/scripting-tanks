export interface LobbyDataDto {
  lid: string
  name: string
  players: number
  joinable: boolean
}

export interface PlayerDataDto {
  cid: string
  name: string
}

interface FullGameWaitingStateDto {
  location: "game-waiting"
  name: string
  owner: string
  players: PlayerDataDto[]
}

interface FullGamePlayingStateDto {
  location: "game-playing"
}

export type FullGameStateDto = FullGameWaitingStateDto | FullGamePlayingStateDto

export interface FullMenuStateDto {
  location: "menu"
  lobbies: LobbyDataDto[]
}

export type FullRoomStateDto = FullGameStateDto | FullMenuStateDto
