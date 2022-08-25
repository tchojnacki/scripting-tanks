const SKY_HEIGHT = 1024

export function SceneLight({ radius = 0 }: { radius?: number }) {
  return (
    <>
      <color attach="background" args={[0.7, 0.9, 1.0]} />
      <ambientLight intensity={0.2} />
      <pointLight
        position={[0, SKY_HEIGHT, 0]}
        intensity={0.3}
        distance={SKY_HEIGHT + radius}
        castShadow
        shadow-mapSize-height={2048}
        shadow-mapSize-width={2048}
      />
    </>
  )
}
