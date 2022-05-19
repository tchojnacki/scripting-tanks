import { useEffect, useRef, useState } from "react"

const MOVEMENT_KEYS = ["w", "a", "s", "d"]

export function useInput() {
  const [pressed, setPressed] = useState<Set<string>>(new Set())
  const [cameraLocked, setCameraLocked] = useState(false)
  const canvasRef = useRef<HTMLCanvasElement | null>(null)
  const [pitch, setPitch] = useState(0)

  useEffect(() => {
    canvasRef.current?.requestPointerLock()
    return () => document.exitPointerLock()
  }, [])

  useEffect(() => {
    const canvas = canvasRef.current

    const listeners = {
      pointerlockchange: () => setCameraLocked(document.pointerLockElement === canvas),
      mousemove: (e: MouseEvent) => {
        if (cameraLocked) setPitch(prev => prev - e.movementX * 0.001)
      },
      click: () => {
        if (!cameraLocked) canvas?.requestPointerLock()
      },
      keydown: (e: KeyboardEvent) => {
        if (MOVEMENT_KEYS.includes(e.key)) setPressed(prev => new Set(prev).add(e.key))
      },
      keyup: (e: KeyboardEvent) => {
        if (MOVEMENT_KEYS.includes(e.key))
          setPressed(prev => new Set([...prev].filter(k => k !== e.key)))
      },
      blur: () => setPressed(new Set()),
    } as const

    Object.entries(listeners).forEach(([event, listener]) =>
      document.addEventListener(event as any, listener)
    )

    return () =>
      Object.entries(listeners).forEach(([event, listener]) =>
        document.removeEventListener(event as any, listener)
      )
  }, [cameraLocked])

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

  const inputAxes = { vertical, horizontal }

  return { canvasRef, pitch, inputAxes }
}
