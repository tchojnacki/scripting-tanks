import { Cylinder, PerspectiveCamera, Plane } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { useIdentity } from "../../utils/indentityContext"
import { useInput } from "../../utils/input"
import { useSocketContext } from "../../utils/socketContext"
import { Tank } from "./components"

const CAMERA_OFFSET = 256

export function Game() {
  const { roomState, useSocketEvent } = useSocketContext<"game-playing">()
  const { cid } = useIdentity()

  const player = roomState.entities.find(e => e.cid === cid)!

  useInput()

  useSocketEvent("s-entity-update", (data, draft) => {
    const idx = draft.entities.findIndex(({ eid }) => eid === data.eid)
    draft.entities[idx] = data
  })

  const cameraFar = CAMERA_OFFSET + roomState.radius * 3

  return (
    <Canvas shadows>
      <color attach="background" args={[0.7, 0.9, 1.0]} />
      <PerspectiveCamera
        position={[
          player.pos[0] - Math.sin(player.pitch) * CAMERA_OFFSET,
          CAMERA_OFFSET / 2,
          player.pos[2] - Math.cos(player.pitch) * CAMERA_OFFSET,
        ]}
        rotation={[0, player.pitch + Math.PI, 0]}
        fov={75}
        near={1}
        far={cameraFar}
        makeDefault
      >
        <Plane
          args={[cameraFar * 4, cameraFar * 4]}
          position={[0, -192, 0]}
          rotation={[-Math.PI / 2, 0, 0]}
        >
          <meshBasicMaterial color="#064273" />
        </Plane>
      </PerspectiveCamera>
      <ambientLight intensity={0.2} />
      <pointLight
        position={[0, 1024, 0]}
        intensity={0.3}
        distance={1024 + roomState.radius}
        castShadow
        shadow-mapSize-height={2048}
        shadow-mapSize-width={2048}
      />
      <Cylinder
        args={[roomState.radius, roomState.radius, 64, 64]}
        position={[0, -32, 0]}
        receiveShadow
      >
        <meshLambertMaterial color="#C2B280" />
      </Cylinder>
      {roomState.entities.map(entity => (
        <Tank key={entity.eid} entity={entity} />
      ))}
    </Canvas>
  )
}
