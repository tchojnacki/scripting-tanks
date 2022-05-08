import { useEffect, useState } from "react"
import { useSocketContext } from "./socketContext"

export function usePlayerName() {
  const { useSocketEvent, sendMessage } = useSocketContext()

  const [name, setName] = useState("")

  useSocketEvent("assign-display-name", data => {
    setName(data as string)
  })

  useEffect(() => {
    sendMessage("refetch-display-name")
  }, [])

  return name
}
