import { Cylinder, MeshWobbleMaterial, PerspectiveCamera, Plane, Sphere } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { useEffect, useState } from "react"
import { BulletDataDto, TankDataDto } from "../../utils/dtos"
import { useIdentity } from "../../utils/indentityContext"
import { useInput } from "../../utils/useInput"
import { useSocketContext } from "../../utils/socketContext"
import { Tank, HUD, SceneLight, Scoreboard } from "../../components"
import { useDocumentTitle } from "@mantine/hooks"

const CAMERA_OFFSET = 256

type N3 = [number, number, number]

export function Game() {
  const { roomState, sendMessage } = useSocketContext<"game-playing">()
  const { cid } = useIdentity()

  const tanks = roomState.entities.filter(e => e.kind === "tank") as TankDataDto[]
  const bullets = roomState.entities.filter(e => e.kind === "bullet") as BulletDataDto[]

  const [spectateTarget, setSpectateTarget] = useState(0)
  const player = tanks.find(e => e.cid === cid) ??
    tanks[spectateTarget % tanks.length] ?? { pos: [0, 0, 0], pitch: 0 }
  const isSpectating = !tanks.some(e => e.cid === cid)

  useDocumentTitle(`${isSpectating ? "Spectating" : "Playing"} | Tanks`)

  const { canvasRef, pitch, inputAxes } = useInput({
    handleShot: () => {
      if (isSpectating) {
        setSpectateTarget(prev => prev + 1)
      } else {
        sendMessage("c-shoot", null)
      }
    },
  })

  const aimPitch = player.pitch + pitch
  const cameraPos = [
    player.pos[0] - Math.sin(aimPitch) * CAMERA_OFFSET,
    Math.max(player.pos[1] + CAMERA_OFFSET / 2, 8),
    player.pos[2] - Math.cos(aimPitch) * CAMERA_OFFSET,
  ] as N3
  const cameraRot = [0, aimPitch + Math.PI, 0] as N3
  const cameraFar = CAMERA_OFFSET + roomState.radius * 3

  useEffect(() => {
    sendMessage("c-set-input-axes", inputAxes)
  }, [inputAxes.vertical, inputAxes.horizontal, sendMessage])

  useEffect(() => {
    sendMessage("c-set-barrel-target", aimPitch)
  }, [aimPitch, sendMessage])

  return (
    <>
      {isSpectating && (
        <HUD justifyContent="flex-start">
          <div style={{ color: "#FF4136", fontSize: 64, paddingTop: 64 }}>SPECTATING</div>
        </HUD>
      )}
      <HUD justifyContent="flex-start" alignItems="flex-end">
        <Scoreboard scoreboard={roomState.scoreboard} focus={cid} />
      </HUD>
      <Canvas shadows ref={canvasRef}>
        <PerspectiveCamera
          position={cameraPos}
          rotation={cameraRot}
          fov={75}
          near={1}
          far={cameraFar}
          makeDefault
        >
          <Plane
            args={[cameraFar * 4, cameraFar * 4]}
            position={[0, -cameraPos[1] - 64, 0]}
            rotation={[-Math.PI / 2, 0, 0]}
          >
            <meshLambertMaterial color="#517cdb" />
          </Plane>
        </PerspectiveCamera>
        <SceneLight />
        <Cylinder
          args={[roomState.radius, roomState.radius, 64, 64]}
          position={[0, -32, 0]}
          receiveShadow
        >
          <meshLambertMaterial color="#C2B280" />
        </Cylinder>
        {tanks.map(tank => (
          <Tank key={tank.eid} tank={{ ...tank, name: tank.cid === cid ? undefined : tank.name }} />
        ))}
        {bullets.map(bullet => (
          <Sphere
            key={bullet.eid}
            position={bullet.pos}
            args={[bullet.radius, 8, 8]}
            castShadow
            receiveShadow
          >
            <meshLambertMaterial color={bullet.owner === cid ? "#060" : "#600"} />
          </Sphere>
        ))}
      </Canvas>
    </>
  )
}
