export interface AssignDisplayNameDto {
  name: string
}

interface FullGameStateDto {
  location: string
  name: string
  owner: string
  players: Record<string, string>[]
}

interface FullMenuStateDto {
  location: string
  lobbies: Record<string, string>[]
}

export type FullRoomStateDto = FullGameStateDto | FullMenuStateDto

export interface LobbyRemovedDto {
  lid: string
}

export interface NewLobbyDto {
  lid: string
  name: string
  players: number
}

export interface NewPlayerDto {
  cid: string
  name: string
}

export interface OwnerChangeDto {
  cid: string
}

export interface PlayerLeftDto {
  cid: string
}
