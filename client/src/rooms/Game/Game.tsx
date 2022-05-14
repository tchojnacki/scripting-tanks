import { PerspectiveCamera } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { useSocketContext } from "../../utils/socketContext"

export function Game() {
  const { roomState } = useSocketContext<"game-playing">()

  return (
    <Canvas shadows>
      <color attach="background" args={[0.7, 0.9, 1.0]} />
      <PerspectiveCamera
        position={[0, 7.5, 12.5]}
        rotation={[-Math.PI / 4, 0, 0]}
        fov={90}
        makeDefault
      />
      <ambientLight intensity={0.25} />
      <pointLight position={[30, 40, 20]} intensity={0.5} castShadow />
      {roomState.entities.map(entity => (
        <mesh key={entity.eid} position={[entity.x, 0, entity.y]} castShadow receiveShadow>
          <boxGeometry args={[1, 1, 1]} />
          <meshStandardMaterial color={entity.color} />
        </mesh>
      ))}
      <mesh position={[0, -1, 0]} receiveShadow>
        <cylinderGeometry args={[11, 11, 1, 64]} />
        <meshStandardMaterial color="#C2B280" />
      </mesh>
    </Canvas>
  )
}
