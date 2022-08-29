import { createContext, ReactNode, useContext, useState } from "react"
import { PlayerDataDto } from "./dtos"
import { useSocketContext } from "./socketContext"

const IdentityContext = createContext<PlayerDataDto>({} as PlayerDataDto)

export function IdentityContextProvider({ children }: { children: ReactNode }) {
  const [identity, setIdentity] = useState<PlayerDataDto>({
    name: "LOADING...",
    cid: "",
    colors: ["#000000", "#000000"],
    bot: false,
  })
  const { useSocketEvent } = useSocketContext()

  useSocketEvent("s-assign-identity", setIdentity)

  return <IdentityContext.Provider value={identity}>{children}</IdentityContext.Provider>
}

export function useIdentity() {
  return useContext(IdentityContext)
}
