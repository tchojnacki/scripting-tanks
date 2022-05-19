export interface LobbyDataDto {
  lid: string
  name: string
  players: number
  location: "game-playing" | "game-waiting"
}

export interface PlayerDataDto {
  cid: string
  name: string
}

export interface TankDataDto {
  kind: "tank"
  eid: string
  cid: string
  color: string
  pos: [number, number, number]
  pitch: number
  barrel: number
}

export interface BulletDataDto {
  kind: "bullet"
  eid: string
  pos: [number, number, number]
  radius: number
}

export type EntityDataDto = TankDataDto | BulletDataDto

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
  radius: number
  entities: EntityDataDto[]
}

export type FullGameStateDto = FullGameWaitingStateDto | FullGamePlayingStateDto

interface FullMenuStateDto {
  location: "menu"
  lobbies: LobbyDataDto[]
}

export type FullRoomStateDto = FullGameStateDto | FullMenuStateDto
