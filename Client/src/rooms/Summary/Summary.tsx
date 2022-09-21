import { Cylinder, PerspectiveCamera } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { Fragment } from "react"
import { useSocketContext } from "../../utils/socketContext"
import { Tank, HUD, SceneLight, Scoreboard } from "../../components"
import { useDocumentTitle } from "@mantine/hooks"
import { useIdentity } from "../../utils/indentityContext"

export function Summary() {
  useDocumentTitle("Post-game | Tanks")
  const { cid } = useIdentity()
  const { roomState } = useSocketContext<"game-summary">()

  return (
    <>
      <HUD justifyContent="flex-end">
        <Scoreboard scoreboard={roomState.scoreboard} focus={cid} />
        <div style={{ fontSize: 32, paddingBlock: 32 }}>{roomState.remaining}s</div>
      </HUD>
      <Canvas shadows>
        <SceneLight />
        <PerspectiveCamera
          position={[1.28, 1.96, -1.28]}
          rotation={[-Math.PI / 6, Math.PI / 12, Math.PI / 24]}
          fov={75}
          near={0.01}
          far={1024}
          makeDefault
        />
        {roomState.tanks.map(tank => (
          <Fragment key={tank.eid}>
            <Tank tank={tank} />
            <Cylinder
              position={[tank.pos[0], tank.pos[1] / 2, tank.pos[2]]}
              args={[1.28, 1.28, tank.pos[1], 32]}
              castShadow
              receiveShadow
            >
              <meshLambertMaterial color="#888888" />
            </Cylinder>
          </Fragment>
        ))}
        <Cylinder args={[5.12, 5.12, 0.64, 32]} position={[0, -0.32, -2.56]} receiveShadow>
          <meshLambertMaterial color="#85BA83" />
        </Cylinder>
      </Canvas>
    </>
  )
}
