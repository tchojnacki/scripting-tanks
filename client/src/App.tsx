import { PerspectiveCamera } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { useEffect, useRef, useState } from "react"
import { WEBSOCKET_ROOT } from "./config"
import { color, position } from "./util"

function Box({
  position = [0, 0, 0],
  size = [1, 1, 1],
  color = "black",
}: {
  position?: [number, number, number]
  size?: [number, number, number]
  color?: string
} = {}) {
  return (
    <mesh position={position} castShadow receiveShadow>
      <boxGeometry args={size} />
      <meshLambertMaterial color={color} />
    </mesh>
  )
}

function App() {
  const wsRef = useRef<WebSocket>()

  const [players, setPlayers] = useState<number[]>([])

  useEffect(() => {
    const ws = new WebSocket(`${WEBSOCKET_ROOT}/game`)

    ws.onmessage = event => {
      const message = JSON.parse(event.data)

      if (message.tag === "full_state") {
        setPlayers(message.data)
      } else if (message.tag === "connected") {
        setPlayers(prev => (prev.includes(message.data) ? prev : [...prev, message.data]))
      } else if (message.tag === "disconnected") {
        setPlayers(prev => prev.filter(p => p !== message.data))
      }
    }

    wsRef.current = ws

    return () => {
      ws.close()
    }
  }, [])

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
      <Box position={[0, -1, 0]} size={[21, 1, 21]} color="#3D9970" />
      {players.map(player => (
        <Box key={player} color={color(player)} position={position(player)} />
      ))}
    </Canvas>
  )
}

export default App
