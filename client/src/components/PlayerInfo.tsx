import { ActionIcon, AspectRatio, Avatar, ColorInput, Group, Stack, Title } from "@mantine/core"
import { PerspectiveCamera } from "@react-three/drei"
import { Canvas, useFrame } from "@react-three/fiber"
import { useState } from "react"
import { ArrowsShuffle } from "tabler-icons-react"
import { useIdentity } from "../utils/indentityContext"
import { Tank } from "./Tank"

const TANK_ROT_SPEED = 0.5
const BARREL_ROT_SPEED = 0.6

function nameAbbr(name: string) {
  return name
    .split(" ")
    .map(word => word[0])
    .join("")
}

function DisplayTank() {
  const [pitch, setPitch] = useState(0)
  const [barrel, setBarrel] = useState(0)

  useFrame((_, delta) => {
    setPitch(prev => prev + delta * TANK_ROT_SPEED)
    setBarrel(prev => prev + delta * BARREL_ROT_SPEED)
  })

  return (
    <Tank
      tank={{
        pos: [0, 0, 0],
        color: "red",
        pitch,
        barrel,
      }}
    />
  )
}

interface PlayerInfoProps {
  compact?: boolean
  unmutable?: boolean
}

export function PlayerInfo({ compact, unmutable }: PlayerInfoProps) {
  const { name } = useIdentity()

  return (
    <div>
      <Title order={3}>Player info</Title>
      <Stack align="center" py="xl">
        <Group>
          <Avatar color="blue" radius="xl" size={24}>
            {nameAbbr(name)}
          </Avatar>
          <Title order={4}>{name}</Title>
          {!unmutable && (
            <ActionIcon variant="filled">
              <ArrowsShuffle size={16} />
            </ActionIcon>
          )}
        </Group>
        <div>
          {!compact && (
            <>
              <AspectRatio ratio={1} sx={{ width: "100vh", maxWidth: 250 }}>
                <div>
                  <Canvas>
                    <ambientLight intensity={0.2} />
                    <pointLight position={[256, 256, 256]} intensity={0.3} />
                    <PerspectiveCamera
                      makeDefault
                      position={[0, 64, 192]}
                      rotation={[-Math.PI / 16, 0, 0]}
                    />
                    <DisplayTank />
                  </Canvas>
                </div>
              </AspectRatio>
            </>
          )}
          <ColorInput label="Tank" disabled={unmutable} size="xs" />
          <ColorInput label="Turret" disabled={unmutable} size="xs" />
        </div>
      </Stack>
    </div>
  )
}
