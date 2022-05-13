import React from "react"
import ReactDOM from "react-dom/client"
import "./index.css"
import { App } from "./App"
import { SocketContextProvider } from "./utils/socketContext"
import { IdentityContextProvider } from "./utils/indentityContext"

const root = ReactDOM.createRoot(document.getElementById("root") as HTMLElement)
root.render(
  <React.StrictMode>
    <SocketContextProvider>
      <IdentityContextProvider>
        <App />
      </IdentityContextProvider>
    </SocketContextProvider>
  </React.StrictMode>
)
