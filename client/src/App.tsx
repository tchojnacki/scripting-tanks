import { useEffect, useRef } from "react"

const SERVER_HOST = process.env.NODE_ENV === "production" ? window.location.host : "localhost:3000"

const WEBSOCKET_ROOT = `${
  window.location.protocol === "https:" ? "wss:" : "ws:"
}//${SERVER_HOST}/ws`
const API_ROOT = `${window.location.protocol}//${SERVER_HOST}/api`

function App() {
  const wsRef = useRef<WebSocket>()

  useEffect(() => {
    const ws = new WebSocket(`${WEBSOCKET_ROOT}/game`)

    ws.onmessage = ({ data }) => {
      console.log(JSON.parse(data))
    }

    wsRef.current = ws

    return () => {
      ws.close()
    }
  }, [])

  useEffect(() => {
    fetch(`${API_ROOT}/hello`)
      .then(resp => resp.json())
      .then(json => console.log(json))
  }, [])

  return <></>
}

export default App
