import { PerspectiveCamera } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { useInput } from "../../utils/input"
import { useSocketContext } from "../../utils/socketContext"

export function Game() {
  const { roomState, useSocketEvent } = useSocketContext<"game-playing">()

  useInput()

  useSocketEvent("s-entity-update", (data, draft) => {
    const idx = draft.entities.findIndex(({ eid }) => eid === data.eid)
    draft.entities[idx] = data
  })

  return (
    <Canvas shadows>
      <color attach="background" args={[0.7, 0.9, 1.0]} />
      <PerspectiveCamera
        position={[0, 500, 750]}
        rotation={[-Math.PI / 4, 0, 0]}
        fov={90}
        far={2048}
        makeDefault
      />
      <ambientLight intensity={0.25} />
      <pointLight position={[0, 512, 0]} intensity={0.5} distance={2048} castShadow />
      {roomState.entities.map(entity => (
        <mesh
          key={entity.eid}
          position={[entity.x, 32, entity.z]}
          rotation={[0, -entity.angle, 0]}
          castShadow
          receiveShadow
        >
          <boxGeometry args={[64, 64, 64]} />
          <meshStandardMaterial color={entity.color} />
        </mesh>
      ))}
      <mesh position={[0, -32, 0]} receiveShadow>
        <cylinderGeometry args={[600, 600, 64, 256]} />
        <meshStandardMaterial color="#C2B280" />
      </mesh>
    </Canvas>
  )
}
