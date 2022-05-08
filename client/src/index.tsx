import React from "react"
import ReactDOM from "react-dom/client"
import "./index.css"
import { App } from "./App"
import { SocketContextProvider } from "./socketContext"

const root = ReactDOM.createRoot(document.getElementById("root") as HTMLElement)
root.render(
  <React.StrictMode>
    <SocketContextProvider>
      <App />
    </SocketContextProvider>
  </React.StrictMode>
)
