export function Menu({ name, roomState, sendMessage }: any) {
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
}
