import { ReactNode } from "react"

interface OverlayProps {
  children: ReactNode
  justifyContent?: "flex-start" | "center" | "flex-end"
  alignItems?: "flex-start" | "center" | "flex-end"
}

export function HUD({ children, justifyContent = "center", alignItems = "center" }: OverlayProps) {
  return (
    <div
      style={{
        pointerEvents: "none",
        position: "fixed",
        top: 0,
        left: 0,
        bottom: 0,
        right: 0,
        zIndex: 1,
        color: "white",
        padding: 16,
        display: "flex",
        flexDirection: "column",
        justifyContent,
        alignItems,
      }}
    >
      {children}
    </div>
  )
}
