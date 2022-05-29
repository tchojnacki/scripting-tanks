import { AppShell, Header, SimpleGrid, Title, Group } from "@mantine/core"
import { useMediaQuery } from "@mantine/hooks"
import { ReactElement, ReactNode } from "react"

interface StandardLayoutProps {
  title: string
  headerRight: ReactNode
  children: [ReactElement, ReactElement]
}

export function StandardLayout({ title, headerRight, children }: StandardLayoutProps) {
  const isMobile = useMediaQuery("(max-width: 500px)")

  return (
    <AppShell
      header={
        <Header height={64}>
          <Group sx={{ height: "100%" }} px={isMobile ? 8 : 40} position="apart">
            <Title order={2}>{title}</Title>
            {headerRight}
          </Group>
        </Header>
      }
    >
      <SimpleGrid cols={isMobile ? 1 : 2} spacing="xl" p="xl">
        {children}
      </SimpleGrid>
    </AppShell>
  )
}
