const SERVER_HOST =
  process.env.NODE_ENV === "production" ? window.location.host : `${window.location.hostname}:3000`

export const WEBSOCKET_ROOT = `wss://${SERVER_HOST}/ws`
