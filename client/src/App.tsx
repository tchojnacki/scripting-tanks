import { PerspectiveCamera } from "@react-three/drei"
import { Canvas } from "@react-three/fiber"
import { useEffect, useRef, useState } from "react"
import { WEBSOCKET_ROOT } from "./config"

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

function Game() {
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
    </Canvas>
  )
}

function App() {
  const wsRef = useRef<WebSocket>()

  const [roomState, setRoomState] = useState<any>({
    location: "menu",
    lobbies: [],
  })

  const [name, setName] = useState("")

  useEffect(() => {
    const ws = new WebSocket(`${WEBSOCKET_ROOT}/game`)

    ws.onmessage = event => {
      const message = JSON.parse(event.data)

      if (message.tag === "assign-display-name") {
        setName(message.data)
      } else if (message.tag === "full-room-state") {
        setRoomState(message.data)
      } else if (message.tag === "new-lobby") {
        setRoomState((prev: any) => ({ ...prev, lobbies: [...prev.lobbies, message.data] }))
      } else if (message.tag === "lobby-removed") {
        setRoomState((prev: any) => ({
          ...prev,
          lobbies: prev.lobbies.filter(({ lid }: any) => lid !== message.data.lid),
        }))
      } else if (message.tag === "new-player") {
        setRoomState((prev: any) => ({ ...prev, players: [...prev.players, message.data] }))
      } else if (message.tag === "owner-change") {
        setRoomState((prev: any) => ({ ...prev, owner: message.data }))
      } else if (message.tag === "player-left") {
        setRoomState((prev: any) => ({
          ...prev,
          players: prev.players.filter((p: any) => p.cid !== message.data),
        }))
      }
    }

    wsRef.current = ws

    return () => {
      ws.close()
    }
  }, [])

  function sendMessage(tag: string, data: any = null) {
    wsRef.current?.send(JSON.stringify({ tag, data }))
  }

  if (roomState.location === "menu") {
    return (
      <>
        <h5>Your name: {name}</h5>
        <button onClick={() => sendMessage("create-lobby")}>Create lobby</button>
        <h1>Menu</h1>
        <h2>Lobbies</h2>
        <ul>
          {roomState?.lobbies?.map((lobby: any) => (
            <li key={lobby.lid}>
              {lobby.name} {lobby.players}{" "}
              <button onClick={() => sendMessage("enter-lobby", lobby.lid)}>Enter</button>
            </li>
          ))}
        </ul>
      </>
    )
  } else if (roomState.location === "lobby") {
    return (
      <>
        <h5>Your name: {name}</h5>
        <button onClick={() => sendMessage("leave-lobby")}>Leave</button>
        <h1>{roomState?.name}</h1>
        <h2>Players</h2>
        <ul>
          {roomState?.players?.map((player: any) => (
            <li key={player.cid}>
              {player.name} {player.cid === roomState?.owner && "👑"}
            </li>
          ))}
        </ul>
      </>
    )
  } else {
    return null
  }
}

export default App
