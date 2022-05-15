import { Box, Cylinder, PerspectiveCamera, Plane } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { useIdentity } from "../../utils/indentityContext"
import { useInput } from "../../utils/input"
import { useSocketContext } from "../../utils/socketContext"

export function Game() {
  const { roomState, useSocketEvent } = useSocketContext<"game-playing">()
  const { cid } = useIdentity()

  const player = roomState.entities.find(e => e.cid === cid)!

  useInput()

  useSocketEvent("s-entity-update", (data, draft) => {
    const idx = draft.entities.findIndex(({ eid }) => eid === data.eid)
    draft.entities[idx] = data
  })

  return (
    <Canvas shadows>
      <color attach="background" args={[0.7, 0.9, 1.0]} />
      <PerspectiveCamera
        position={[
          player.x - Math.sin(player.pitch) * 256,
          128,
          player.z - Math.cos(player.pitch) * 256,
        ]}
        rotation={[0, player.pitch + Math.PI, 0]}
        fov={75}
        near={1}
        far={2048}
        makeDefault
      >
        <Plane args={[8192, 8192]} position={-192} rotation={[-Math.PI / 2, 0, 0]}>
          <meshBasicMaterial color="#064273" />
        </Plane>
      </PerspectiveCamera>
      <ambientLight intensity={0.25} />
      <pointLight
        position={[0, 1024, 0]}
        intensity={0.5}
        distance={2048}
        castShadow
        shadow-mapSize-height={1024}
        shadow-mapSize-width={1024}
      />
      <Cylinder args={[600, 600, 64, 256]} position={[0, -32, 0]} receiveShadow>
        <meshLambertMaterial color="#C2B280" />
      </Cylinder>
      {roomState.entities.map(entity => (
        <Box
          key={entity.eid}
          args={[64, 64, 64]}
          position={[entity.x, 32, entity.z]}
          rotation={[0, entity.pitch, 0]}
          castShadow
          receiveShadow
        >
          <meshLambertMaterial color={entity.color} />
          <Box args={[16, 16, 32]} position={[0, 0, 48]} castShadow receiveShadow>
            <meshLambertMaterial color={entity.color} />
          </Box>
        </Box>
      ))}
    </Canvas>
  )
}
