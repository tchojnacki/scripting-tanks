function numberHash(n: number) {
  n = ((n >> 16) ^ n) * 0x45d9f3b
  n = ((n >> 16) ^ n) * 0x45d9f3b
  n = (n >> 16) ^ n
  return n
}

export function color(n: number) {
  return "#" + (numberHash(n) % 256 ** 3).toString(16).padStart(6, "0")
}

export function position(n: number) {
  const SIDE = 21
  const slot = numberHash(n) % SIDE ** 2
  const x = Math.floor(slot / SIDE) - Math.floor(SIDE / 2)
  const z = (slot % SIDE) - Math.floor(SIDE / 2)
  return [x, 0, z] as [number, number, number]
}
