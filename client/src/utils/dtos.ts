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

export interface EntityDataDto {
  eid: string
  kind: "tank"
  color: string
  x: number
  z: number
  angle: number
}

export interface InputAxesDto {
  vertical: number
  horizontal: number
}

interface FullGameWaitingStateDto {
  location: "game-waiting"
  name: string
  owner: string
  players: PlayerDataDto[]
}

interface FullGamePlayingStateDto {
  location: "game-playing"
  entities: EntityDataDto[]
}

export type FullGameStateDto = FullGameWaitingStateDto | FullGamePlayingStateDto

interface FullMenuStateDto {
  location: "menu"
  lobbies: LobbyDataDto[]
}

export type FullRoomStateDto = FullGameStateDto | FullMenuStateDto
