import { Box, Cylinder, Text } from "@react-three/drei"
import { TankDataDto } from "../utils/dtos"
import { useLookAt } from "../utils/useLookAt"

function Catterpillar({ color, position }: { color: string; position: [number, number, number] }) {
  return (
    <group position={position}>
      <Box args={[16, 24, 96]} castShadow receiveShadow>
        <meshLambertMaterial color="#444" />
      </Box>
      {[1, -1].map(front => (
        <Cylinder
          key={front}
          args={[12, 12, 16, 8, 1, false, 0, Math.PI]}
          rotation={[(front * Math.PI) / 2, 0, Math.PI / 2]}
          position={[0, 0, front * 48]}
        >
          <meshLambertMaterial color="#444" />
        </Cylinder>
      ))}
      <Box args={[16, 8, 112]} position={[0, 16, 0]}>
        <meshLambertMaterial color={color} />
      </Box>
    </group>
  )
}

function Turret({
  color,
  position,
  rotation,
}: {
  color: string
  position: [number, number, number]
  rotation: number
}) {
  return (
    <group position={position} rotation={[0, rotation, 0]}>
      <Cylinder args={[24, 32, 24, 16]}>
        <meshLambertMaterial color={color} />
      </Cylinder>
      <Cylinder args={[8, 8, 48, 8]} rotation={[Math.PI / 2, 0, 0]} position={[0, 0, 48]}>
        <meshLambertMaterial color={color} />
      </Cylinder>
    </group>
  )
}

interface TankProps {
  tank: Pick<TankDataDto, "name" | "pos" | "colors" | "pitch" | "barrel">
}

export function Tank({ tank }: TankProps) {
  const textRef = useLookAt()

  return (
    <group position={tank.pos} rotation={[0, tank.pitch, 0]}>
      <Box args={[64, 32, 96]} position={[0, 32, 0]} castShadow receiveShadow>
        <meshLambertMaterial color={tank.colors[0]} />
      </Box>
      <Turret color={tank.colors[1]} position={[0, 60, 0]} rotation={tank.barrel - tank.pitch} />
      {[1, -1].map(side => (
        <Catterpillar key={side} color={tank.colors[0]} position={[side * 40, 12, 0]} />
      ))}
      {tank.name && (
        <Text color="#FF4136" scale={128} position={[0, 96, 0]} ref={textRef}>
          {tank.name}
        </Text>
      )}
    </group>
  )
}
