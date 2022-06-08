export type TankColors = [string, string]

export interface PlayerDataDto {
  cid: string
  name: string
  colors: TankColors
  bot: boolean
}

export interface CustomColorsDto {
  colors: TankColors
}

export interface TankDataDto {
  kind: "tank"
  eid: string
  cid: string
  name?: string
  colors: TankColors
  pos: [number, number, number]
  pitch: number
  barrel: number
}

export interface BulletDataDto {
  kind: "bullet"
  eid: string
  owner: string
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

export interface ScoreboardEntryDto {
  cid: string
  name: string
  score: number
}

interface FullGamePlayingStateDto {
  location: "game-playing"
  radius: number
  entities: EntityDataDto[]
  scoreboard: ScoreboardEntryDto[]
}

interface FullSummaryStateDto {
  location: "game-summary"
  remaining: number
  tanks: TankDataDto[]
  scoreboard: ScoreboardEntryDto[]
}

export type FullGameStateDto =
  | FullGameWaitingStateDto
  | FullGamePlayingStateDto
  | FullSummaryStateDto

export interface LobbyDataDto {
  lid: string
  name: string
  players: number
  location: FullGameStateDto["location"]
}

interface FullMenuStateDto {
  location: "menu"
  lobbies: LobbyDataDto[]
}

export type FullRoomStateDto = FullGameStateDto | FullMenuStateDto
