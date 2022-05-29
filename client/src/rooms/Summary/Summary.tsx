import { Cylinder, PerspectiveCamera } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { Fragment } from "react"
import { useSocketContext } from "../../utils/socketContext"
import { Tank, HUD, SceneLight } from "../../components"
import { useDocumentTitle } from "@mantine/hooks"

export function Summary() {
  useDocumentTitle("Post-game | Tanks")

  const { roomState } = useSocketContext<"game-summary">()

  return (
    <>
      <HUD justifyContent="flex-end">
        <div style={{ fontSize: 64, paddingBottom: 64 }}>{roomState.remaining}s</div>
      </HUD>
      <Canvas shadows>
        <PerspectiveCamera
          position={[0, 256, 0]}
          rotation={[-Math.PI / 6, 0, 0]}
          fov={75}
          near={1}
          far={1024}
          makeDefault
        />
        <SceneLight radius={512} />
        {roomState.tanks.map(tank => (
          <Fragment key={tank.eid}>
            <Tank tank={tank} />
            <Cylinder
              position={[tank.pos[0], tank.pos[1] / 2, tank.pos[2]]}
              args={[128, 128, tank.pos[1], 32]}
            >
              <meshLambertMaterial color="#888888" />
            </Cylinder>
          </Fragment>
        ))}
        <Cylinder args={[512, 512, 64, 32]} position={[0, -32, -256]}>
          <meshLambertMaterial color="#85BA83" />
        </Cylinder>
      </Canvas>
    </>
  )
}
