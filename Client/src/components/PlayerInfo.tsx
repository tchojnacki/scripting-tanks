import {
  ActionIcon,
  AspectRatio,
  Avatar,
  ColorInput,
  DEFAULT_THEME,
  Group,
  Stack,
  Title,
} from "@mantine/core"
import { PerspectiveCamera } from "@react-three/drei"
import { Canvas, useFrame } from "@react-three/fiber"
import { useState } from "react"
import { ArrowsShuffle, Copy } from "tabler-icons-react"
import { useIdentity } from "../utils/indentityContext"
import { nameAbbr } from "../utils/nameAbbr"
import { useSocketContext } from "../utils/socketContext"
import { Tank } from "./Tank"
import { sample, isEqual } from "lodash"
import { TankColors } from "../utils/dtos"

const TANK_ROT_SPEED = 0.5
const BARREL_ROT_SPEED = 0.6

function DisplayTank({ colors }: { colors: TankColors }) {
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
        colors,
        pitch,
        barrel,
      }}
    />
  )
}

const colorInputProps = {
  size: "xs" as const,
  disallowInput: true,
  withPicker: false,
  swatches: Object.keys(DEFAULT_THEME.colors).flatMap(c => DEFAULT_THEME.colors[c].slice(-5)),
  swatchesPerRow: 10,
}

interface PlayerInfoProps {
  compact?: boolean
  unmutable?: boolean
}

export function PlayerInfo({ compact, unmutable }: PlayerInfoProps) {
  const { sendMessage } = useSocketContext()
  const { name, colors } = useIdentity()

  const setColors = (newColors: TankColors) => {
    if (!isEqual(colors, newColors)) sendMessage("c-customize-colors", { colors: newColors })
  }
  const setTankColor = (color: string) => setColors([color, colors[1]])
  const setTurretColor = (color: string) => setColors([colors[0], color])

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
            <ActionIcon variant="filled" onClick={() => sendMessage("c-reroll-name", null)}>
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
                    <DisplayTank colors={colors} />
                  </Canvas>
                </div>
              </AspectRatio>
            </>
          )}
          <ColorInput
            {...colorInputProps}
            label="Tank"
            disabled={unmutable}
            value={colors[0]}
            onChange={setTankColor}
            rightSection={
              !unmutable && (
                <ActionIcon
                  onClick={() => {
                    const color = sample(colorInputProps.swatches)!
                    setColors([color, color])
                  }}
                >
                  <ArrowsShuffle size={16} />
                </ActionIcon>
              )
            }
          />
          <ColorInput
            {...colorInputProps}
            label="Turret"
            disabled={unmutable}
            value={colors[1]}
            onChange={setTurretColor}
            rightSection={
              !unmutable && (
                <ActionIcon onClick={() => setTurretColor(colors[0])}>
                  <Copy size={16} />
                </ActionIcon>
              )
            }
          />
        </div>
      </Stack>
    </div>
  )
}
