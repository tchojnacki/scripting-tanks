export function Lobby({ roomState, name, sendMessage }: any) {
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
}
