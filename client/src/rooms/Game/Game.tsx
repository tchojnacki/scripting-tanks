import { PerspectiveCamera } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"

export function Game() {
  return (
    <Canvas shadows>
      <PerspectiveCamera
        position={[0, 10, 15]}
        rotation={[-Math.PI / 4, 0, 0]}
        fov={90}
        makeDefault
      />
      <ambientLight intensity={0.25} />
      <pointLight position={[30, 40, 20]} intensity={0.5} castShadow />
      <mesh>
        <boxGeometry args={[5, 5, 5]} />
        <meshStandardMaterial color="hotpink" />
      </mesh>
    </Canvas>
  )
}
