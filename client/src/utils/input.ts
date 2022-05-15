import { useEffect, useState } from "react"
import { useSocketContext } from "./socketContext"

const MOVEMENT_KEYS = ["w", "a", "s", "d"]

export function useInput() {
  const { sendMessage } = useSocketContext()
  const [pressed, setPressed] = useState<Set<string>>(new Set())

  useEffect(() => {
    const keydownListener = (e: KeyboardEvent) =>
      MOVEMENT_KEYS.includes(e.key) && setPressed(prev => new Set(prev).add(e.key))
    const keyupListener = (e: KeyboardEvent) =>
      MOVEMENT_KEYS.includes(e.key) &&
      setPressed(prev => new Set([...prev].filter(k => k !== e.key)))
    const blurListener = () => setPressed(new Set())

    document.addEventListener("keydown", keydownListener)
    document.addEventListener("keyup", keyupListener)
    document.addEventListener("blur", blurListener)

    return () => {
      document.removeEventListener("keydown", keydownListener)
      document.removeEventListener("keyup", keyupListener)
      document.removeEventListener("blur", blurListener)
    }
  }, [])

  useEffect(() => {
    let vertical = 0
    let horizontal = 0

    pressed.forEach(key => {
      ;({
        w: () => (vertical += 1),
        a: () => (horizontal -= 1),
        s: () => (vertical -= 1),
        d: () => (horizontal += 1),
      }?.[key]!?.())
    })

    sendMessage("c-set-input-axes", { vertical, horizontal })
  }, [pressed, sendMessage])
}
