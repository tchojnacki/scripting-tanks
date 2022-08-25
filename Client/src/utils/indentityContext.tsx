import { createContext, ReactNode, useContext, useState } from "react"
import { PlayerDataDto } from "./dtos"
import { useSocketContext } from "./socketContext"

const IdentityContext = createContext<PlayerDataDto>({} as PlayerDataDto)

export function IdentityContextProvider({ children }: { children: ReactNode }) {
  const [identity, setIdentity] = useState<PlayerDataDto>({
    name: "",
    cid: "",
    colors: ["#000000", "#000000"],
  })
  const { useSocketEvent } = useSocketContext()

  useSocketEvent("s-assign-identity", data => {
    setIdentity(data)
  })

  return <IdentityContext.Provider value={identity}>{children}</IdentityContext.Provider>
}

export function useIdentity() {
  return useContext(IdentityContext)
}
