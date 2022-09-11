import { useFrame, useThree } from "@react-three/fiber"
import { useRef } from "react"

export function useLookAt() {
  const targetRef = useRef<any>()
  const { camera } = useThree()

  useFrame(() => {
    targetRef.current?.lookAt(camera.position)
  })

  return targetRef
}
