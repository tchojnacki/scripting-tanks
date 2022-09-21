import { AppShell, Header, Title, Group, Grid } from "@mantine/core"
import { useMediaQuery } from "@mantine/hooks"
import { ReactNode } from "react"

interface StandardLayoutProps {
  title: string
  headerRight: ReactNode
  left: ReactNode
  right: ReactNode
}

export function StandardLayout({ title, headerRight, left, right }: StandardLayoutProps) {
  const isMobile = useMediaQuery("(max-width: 700px)")

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
      styles={theme => ({
        main: {
          backgroundColor:
            theme.colorScheme === "dark" ? theme.colors.dark[8] : theme.colors.gray[1],
        },
      })}
    >
      <Grid grow columns={isMobile ? 2 : 3} gutter="xl" p="xl">
        <Grid.Col span={2}>{left}</Grid.Col>
        <Grid.Col span={1}>{right}</Grid.Col>
      </Grid>
    </AppShell>
  )
}
