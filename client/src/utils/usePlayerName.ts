import { useEffect, useState } from "react"
import { useSocketContext } from "./socketContext"

export function usePlayerName() {
  const { useSocketEvent, sendMessage } = useSocketContext()

  const [name, setName] = useState("")

  useSocketEvent("s-assign-display-name", data => {
    setName(data)
  })

  useEffect(() => {
    sendMessage("c-refetch-display-name", null)
  }, [sendMessage])

  return name
}
