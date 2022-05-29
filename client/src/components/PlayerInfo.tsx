import { useIdentity } from "../utils/indentityContext"

export function PlayerInfo() {
  const { name } = useIdentity()

  return (
    <div>
      <h5>Your name: {name}</h5>
    </div>
  )
}
