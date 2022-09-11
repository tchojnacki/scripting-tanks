import { MantineProvider, Table } from "@mantine/core"
import { ScoreboardEntryDto } from "../utils/dtos"

interface ScoreboardProps {
  scoreboard: ScoreboardEntryDto[]
  focus: string
}

export function Scoreboard({ scoreboard, focus }: ScoreboardProps) {
  return (
    <MantineProvider theme={{ colorScheme: "dark" }}>
      <Table sx={{ width: "auto", background: "rgba(0, 0, 0, 0.75)" }}>
        <thead>
          <tr>
            <th>Name</th>
            <th>Score</th>
          </tr>
        </thead>
        <tbody>
          {scoreboard.map(({ cid, name, score }) => (
            <tr
              key={cid}
              style={{
                backgroundColor: focus === cid ? "rgba(255, 255, 255, 0.1)" : undefined,
              }}
            >
              <td>{name}</td>
              <td align="center">{score}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </MantineProvider>
  )
}
