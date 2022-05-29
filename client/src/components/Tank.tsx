import { Box, Cylinder } from "@react-three/drei"
import { TankDataDto } from "../utils/dtos"

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

export function Tank({ tank }: { tank: TankDataDto }) {
  return (
    <group position={tank.pos} rotation={[0, tank.pitch, 0]}>
      <Box args={[64, 32, 96]} position={[0, 32, 0]} castShadow receiveShadow>
        <meshLambertMaterial color={tank.color} />
      </Box>
      <Turret color={tank.color} position={[0, 64, 0]} rotation={tank.barrel - tank.pitch} />
      {[1, -1].map(side => (
        <Catterpillar key={side} color={tank.color} position={[side * 40, 12, 0]} />
      ))}
    </group>
  )
}