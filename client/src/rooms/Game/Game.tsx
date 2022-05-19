import { Cylinder, PerspectiveCamera, Plane, Text } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { useIdentity } from "../../utils/indentityContext"
import { useInput } from "../../utils/input"
import { useSocketContext } from "../../utils/socketContext"
import { Tank } from "./components"

const CAMERA_OFFSET = 256
const SKY_HEIGHT = 1024

type N3 = [number, number, number]

export function Game() {
  const { roomState } = useSocketContext<"game-playing">()
  const { cid } = useIdentity()
  useInput()

  const player = roomState.entities.find(e => e.cid === cid) ??
    roomState.entities[0] ?? { pos: [0, 0, 0], pitch: 0 }
  const isSpectating = !roomState.entities.some(e => e.cid === cid)

  const cameraPos = [
    player.pos[0] - Math.sin(player.pitch) * CAMERA_OFFSET,
    Math.max(player.pos[1] + CAMERA_OFFSET / 2, 8),
    player.pos[2] - Math.cos(player.pitch) * CAMERA_OFFSET,
  ] as N3
  const cameraRot = [0, player.pitch + Math.PI, 0] as N3
  const cameraFar = CAMERA_OFFSET + roomState.radius * 3

  return (
    <Canvas shadows>
      <color attach="background" args={[0.7, 0.9, 1.0]} />
      <PerspectiveCamera
        position={cameraPos}
        rotation={cameraRot}
        fov={75}
        near={1}
        far={cameraFar}
        makeDefault
      >
        {isSpectating && (
          <Text
            color="red"
            anchorX="center"
            anchorY="middle"
            fontSize={64}
            position={[0, 128, -512]}
          >
            SPECTATING
          </Text>
        )}
        <Plane
          args={[cameraFar * 4, cameraFar * 4]}
          position={[0, -cameraPos[1] - 64, 0]}
          rotation={[-Math.PI / 2, 0, 0]}
        >
          <meshBasicMaterial color="#064273" />
        </Plane>
      </PerspectiveCamera>
      <ambientLight intensity={0.2} />
      <pointLight
        position={[0, SKY_HEIGHT, 0]}
        intensity={0.3}
        distance={SKY_HEIGHT + roomState.radius}
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
