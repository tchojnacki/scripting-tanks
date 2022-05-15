import { createContext, ReactNode, useContext, useState } from "react"
import { PlayerDataDto } from "./dtos"
import { useSocketContext } from "./socketContext"

const IdentityContext = createContext<PlayerDataDto>({ name: "", cid: "" })

export function IdentityContextProvider({ children }: { children: ReactNode }) {
  const [identity, setIdentity] = useState({ name: "", cid: "" })
  const { useSocketEvent } = useSocketContext()

  useSocketEvent("s-assign-identity", data => {
    setIdentity(data)
  })

  return <IdentityContext.Provider value={identity}>{children}</IdentityContext.Provider>
}

export function useIdentity() {
  return useContext(IdentityContext)
}
