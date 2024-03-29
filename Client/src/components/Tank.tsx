import { Billboard, Box, Cylinder, Text } from "@react-three/drei"
import { TankDataDto } from "../utils/dtos"

function Catterpillar({ color, position }: { color: string; position: [number, number, number] }) {
  return (
    <group position={position}>
      <Box args={[0.16, 0.24, 0.96]} castShadow receiveShadow>
        <meshLambertMaterial color="#444" />
      </Box>
      {[1, -1].map(front => (
        <Cylinder
          key={front}
          args={[0.12, 0.12, 0.16, 8, 1, false, 0, Math.PI]}
          rotation={[(front * Math.PI) / 2, 0, Math.PI / 2]}
          position={[0, 0, front * 0.48]}
          castShadow
          receiveShadow
        >
          <meshLambertMaterial color="#444" />
        </Cylinder>
      ))}
      <Box args={[0.16, 0.08, 1.12]} position={[0, 0.16, 0]} castShadow receiveShadow>
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
      <Cylinder args={[0.24, 0.32, 0.24, 16]} castShadow receiveShadow>
        <meshLambertMaterial color={color} />
      </Cylinder>
      <Cylinder
        args={[0.08, 0.08, 0.48, 16]}
        rotation={[Math.PI / 2, 0, 0]}
        position={[0, 0, 0.48]}
        castShadow
        receiveShadow
      >
        <meshLambertMaterial color={color} />
      </Cylinder>
    </group>
  )
}

interface TankProps {
  tank: Pick<TankDataDto, "name" | "pos" | "colors" | "pitch" | "barrel">
}

export function Tank({ tank }: TankProps) {
  return (
    <group position={tank.pos} rotation={[0, tank.pitch, 0]}>
      <Box args={[0.64, 0.32, 0.96]} position={[0, 0.32, 0]} castShadow receiveShadow>
        <meshLambertMaterial color={tank.colors[0]} />
      </Box>
      <Turret color={tank.colors[1]} position={[0, 0.6, 0]} rotation={tank.barrel - tank.pitch} />
      {[1, -1].map(side => (
        <Catterpillar key={side} color={tank.colors[0]} position={[side * 0.4, 0.12, 0]} />
      ))}
      {tank.name && (
        <Billboard>
          <Text color="#FF4136" scale={1.5} position={[0, 1, 0]}>
            {tank.name}
          </Text>
        </Billboard>
      )}
    </group>
  )
}
