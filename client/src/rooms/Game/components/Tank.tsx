import { Box, Cylinder } from "@react-three/drei"
import { EntityDataDto } from "../../../utils/dtos"

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

function Turret({ color, position }: { color: string; position: [number, number, number] }) {
  return (
    <group position={position}>
      <Cylinder args={[24, 32, 24, 16]}>
        <meshLambertMaterial color={color} />
      </Cylinder>
      <Cylinder args={[8, 8, 48]} rotation={[Math.PI / 2, 0, 0]} position={[0, 0, 48]}>
        <meshLambertMaterial color={color} />
      </Cylinder>
    </group>
  )
}

export function Tank({ entity }: { entity: EntityDataDto }) {
  return (
    <group position={entity.pos} rotation={[0, entity.pitch, 0]}>
      <Box args={[64, 32, 96]} position={[0, 32, 0]} castShadow receiveShadow>
        <meshLambertMaterial color={entity.color} />
      </Box>
      <Turret color={entity.color} position={[0, 64, 0]} />
      {[1, -1].map(side => (
        <Catterpillar key={side} color={entity.color} position={[side * 40, 12, 0]} />
      ))}
    </group>
  )
}
