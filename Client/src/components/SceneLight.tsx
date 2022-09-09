export function SceneLight() {
  return (
    <>
      <color attach="background" args={[0.7, 0.9, 1.0]} />
      <ambientLight intensity={0.1} />
      <directionalLight
        position={[1024, 2048, 1024]}
        intensity={0.2}
        castShadow
        shadow-camera-bottom={-2048}
        shadow-camera-top={2048}
        shadow-camera-left={-2048}
        shadow-camera-right={2048}
        shadow-camera-near={1}
        shadow-camera-far={8192}
        shadow-mapSize-height={8192}
        shadow-mapSize-width={8192}
      />
    </>
  )
}
