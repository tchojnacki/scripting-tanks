import { useThree } from "@react-three/fiber"
import { useRef } from "react"
import { DirectionalLight } from "three"

export function SceneLight() {
  const lightRef = useRef<DirectionalLight>(null)
  useThree(({ camera }) => {
    const light = lightRef.current
    if (light) {
      light.target.position.set(camera.position.x, 0, camera.position.z)
      light.target.updateMatrixWorld()
      light.position.set(camera.position.x + 100, 200, camera.position.z + 100)
    }
  })

  return (
    <>
      <color attach="background" args={[0.7, 0.9, 1.0]} />
      <ambientLight intensity={0.1} />
      <directionalLight
        ref={lightRef}
        intensity={0.2}
        castShadow
        shadow-camera-bottom={-16}
        shadow-camera-top={16}
        shadow-camera-left={-16}
        shadow-camera-right={16}
        shadow-mapSize-height={8192}
        shadow-mapSize-width={8192}
      />
    </>
  )
}
