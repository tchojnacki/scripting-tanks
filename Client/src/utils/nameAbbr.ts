export function nameAbbr(name: string) {
  return name
    .split(" ")
    .map(word => word[0])
    .slice(0, 2)
    .join("")
}
